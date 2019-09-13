using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreLoader.Generator.OpenGL
{
    public class BindingCodeGenerator
    {
        private static readonly Regex EntityRegex = new Regex("&[a-zA-Z0-9]+;");
        private readonly string _xmlFileName;
        private XDocument _xDocument;
        private XNamespace _rootNamespace;

        public BindingCodeGenerator(string xmlFileName)
        {
            _xmlFileName = xmlFileName;
        }

        public async Task GenerateCodeAsync(BindingCodeUnit codeUnit, CodeAnalyzer codeAnalyzer, CodeAnalyzer nativeCodeAnalyzer, EnumGenerator enumGenerator)
        {
            using (var fileStream = File.Open(_xmlFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();
                content = EntityRegex.Replace(content, "");
                content = content.Replace("xlink:href", "href");

                _xDocument = XDocument.Parse(content);
                _rootNamespace = _xDocument.Root.GetDefaultNamespace();

                foreach (var func in _xDocument.Descendants(_rootNamespace + "funcprototype"))
                {
                    GenerateFunction(codeUnit, codeAnalyzer, nativeCodeAnalyzer, enumGenerator, func);
                }
            }
        }

        private void GenerateFunction(BindingCodeUnit codeUnit, CodeAnalyzer codeAnalyzer, CodeAnalyzer nativeCodeAnalyzer, EnumGenerator enumGenerator, XElement funcElement)
        {
            var funcDef = funcElement.Descendants(_rootNamespace + "funcdef").First();
            var functionName = funcDef.Descendants(_rootNamespace + "function").First().Value;

            if (functionName.StartsWith("gl"))
                functionName = functionName.Substring(2);

            var returnType = TypeHelper.GetType(funcDef.Value, out var enumType);
            //var returnTypeReference = enumType == EnumType.None ? TypeHelper.GetReturnType(returnType) : enumGenerator.CreateEnum(functionName, "return", enumType, null); //todo get enum definition
            var returnTypeReference = TypeHelper.GetReturnType(returnType);

            var delegateName = functionName + "Func";
            var delegateType = new CodeTypeDelegate(delegateName)
            {
                ReturnType = returnTypeReference
            };

            var args = XElementToParams(funcElement);

            if (!nativeCodeAnalyzer.HasField(functionName))
            {
                var parameters = args.Select(param => TypeHelper.GetParameterType(param.Type, param.TypeName, param.Name)).ToArray();
                delegateType.Parameters.AddRange(parameters);
                codeUnit.NativeGeneratedClass.Members.Add(delegateType);

                var field = new CodeMemberField(new CodeTypeReference(delegateName), functionName)
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Static
                };
                codeUnit.NativeGeneratedClass.Members.Add(field);
            }

            if (!codeAnalyzer.HasCallTo($"{BindingCodeUnit.NativeClassName}.{functionName}"))
            {
                var method = new CodeMemberMethod
                {
                    Name = functionName,
                    ReturnType = returnTypeReference,
                    Attributes = MemberAttributes.Public | MemberAttributes.Static
                };
                method.Parameters.AddRange(args.Select(param =>
                {
                    List<string> enumFieldNames = null;
                    if (param.EnumType != EnumType.None)
                        enumFieldNames = GetEnumFieldNames(param.Name);
                    if (enumFieldNames == null || enumFieldNames.Count == 0)
                    {
                        param.EnumType = EnumType.None;
                        return TypeHelper.GetParameterType(param.Type, param.TypeName, param.Name);
                    }
                    return new CodeParameterDeclarationExpression(enumGenerator.CreateEnum(functionName, param.Name, param.EnumType, enumFieldNames), param.Name);
                }).ToArray());
                //adds [MethodImpl(MethodImplOptions.AggressiveInlining)]
                method.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(nameof(MethodImplAttribute)), new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(nameof(MethodImplOptions)), nameof(MethodImplOptions.AggressiveInlining)))));

                var argCalls = args.Select(param =>
                {
                    CodeExpression argExpression = new CodeArgumentReferenceExpression(param.Name);
                    if (param.EnumType != EnumType.None)
                        return new CodeCastExpression(typeof(uint), argExpression);
                    if (param.Type?.IsByRef == true)
                        argExpression = new CodeDirectionExpression(FieldDirection.Ref, argExpression);
                    return argExpression;
                }).ToArray();
                var callStatement = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(BindingCodeUnit.NativeClassName), functionName, argCalls);
                if (returnType != typeof(void))
                    method.Statements.Add(new CodeMethodReturnStatement(callStatement));
                else
                    method.Statements.Add(callStatement);
                codeUnit.ApiGeneratedClass.Members.Add(method);
            }
        }

        private List<ParamData> XElementToParams(XElement element)
        {
            var parameters = new List<ParamData>();
            var args = element.Descendants(_rootNamespace + "paramdef").ToList();

            if (args.Count == 1 && args.First().Value == "void")
                return parameters;

            foreach (var arg in args)
            {
                var type = TypeHelper.GetType(arg.Value, out var enumType);
                var param = new ParamData(arg.Descendants(_rootNamespace + "parameter").First().Value, arg.Value, type, enumType);
                parameters.Add(param);
            }
            return parameters;
        }

        private List<string> GetEnumFieldNames(string paramName)
        {
            try
            {
                var refSection = _xDocument.Descendants(_rootNamespace + "refsect1");
                var paramSection = refSection.First(s => s.Attribute(XNamespace.Xml + "id").Value == "parameters");
                var paramNamespace = paramSection.GetDefaultNamespace();
                var entry = paramSection.Descendants(paramNamespace + "varlistentry").First(e => e.Descendants(paramNamespace + "parameter").First().Value == paramName);
                return entry.Descendants(paramNamespace + "constant").Select(c => c.Value).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}