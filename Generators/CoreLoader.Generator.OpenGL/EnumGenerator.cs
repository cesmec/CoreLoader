using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreLoader.Generator.OpenGL
{
    public class EnumGenerator
    {
        private static readonly Regex FieldNameRegex = new Regex(@"_(\S)");
        private readonly BindingCodeUnit _codeUnit;
        private readonly Dictionary<string, uint> _constants;

        public EnumGenerator(BindingCodeUnit codeUnit, Dictionary<string, uint> constants)
        {
            _codeUnit = codeUnit;
            _constants = constants;
        }

        public CodeTypeReference CreateEnum(string functionName, string paramName, EnumType enumType, List<string> fieldNames)
        {
            var enumName = $"{functionName}{FirstCharUpper(paramName)}";

            var generatedEnum = new CodeTypeDeclaration(enumName)
            {
                BaseTypes = { new CodeTypeReference(typeof(uint)) },
                IsEnum = true
            };
            if (enumType == EnumType.Bitfield)
            {
                var flagsAttribute = new CodeAttributeDeclaration(new CodeTypeReference(nameof(FlagsAttribute)));
                generatedEnum.CustomAttributes.Add(flagsAttribute);
            }
            foreach (var field in fieldNames.Distinct())
            {
                if (!_constants.ContainsKey(field)) //todo should not happen
                {
                    Console.WriteLine($"Missing Constant '{field}'");
                    continue;
                }

                var value = _constants[field];
                var fieldName = GetFieldName(field);
                generatedEnum.Members.Add(new CodeMemberField(enumName, fieldName)
                {
                    InitExpression = new CodePrimitiveExpression(value)
                });
            }
            _codeUnit.ApiGeneratedNamespace.Types.Add(generatedEnum);
            return new CodeTypeReference(enumName);
        }

        private static string GetFieldName(string field)
        {
            field = FirstCharUpper(field.ToLower());
            if (field.StartsWith("Gl_"))
                field = field.Substring(2);
            return FieldNameRegex.Replace(field, match => match.Groups[1].Value.ToUpper());
        }

        private static string FirstCharUpper(string value)
        {
            return value[0].ToString().ToUpper() + value.Substring(1);
        }
    }
}