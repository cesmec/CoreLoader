using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace CoreLoader.Generator.OpenGL
{
    public class BindingCodeUnit
    {
        public const string NativeClassName = "GlNative";
        public const string ApiClassName = "GL";

        public CodeCompileUnit NativeCompileUnit { get; } = new CodeCompileUnit();
        public CodeCompileUnit ApiCompileUnit { get; } = new CodeCompileUnit();
        public CodeNamespace NativeGeneratedNamespace { get; }
        public CodeNamespace ApiGeneratedNamespace { get; }
        public CodeTypeDeclaration NativeGeneratedClass { get; }
        public CodeTypeDeclaration ApiGeneratedClass { get; }

        public BindingCodeUnit()
        {
            (NativeGeneratedNamespace, NativeGeneratedClass) = DeclareClass(NativeCompileUnit, NativeClassName);
            (ApiGeneratedNamespace, ApiGeneratedClass) = DeclareClass(ApiCompileUnit, ApiClassName, "System", "System.Runtime.CompilerServices", $"static CoreLoader.OpenGL.{NativeClassName}");
        }

        public void GenerateClasses(string bindingsProjectPath)
        {
            using (var provider = CodeDomProvider.CreateProvider("CSharp"))
            {
                var options = new CodeGeneratorOptions { BracingStyle = "C" };
                var nativeClassFileName = Path.Combine(bindingsProjectPath, NativeClassName + ".generated.cs");
                using (var sourceWriter = new StreamWriter(nativeClassFileName))
                {
                    provider.GenerateCodeFromCompileUnit(NativeCompileUnit, sourceWriter, options);
                }
                File.WriteAllText(nativeClassFileName, File.ReadAllText(nativeClassFileName, Encoding.UTF8).Replace(" partial class ", " unsafe partial class "));

                var apiClassFileName = Path.Combine(bindingsProjectPath, ApiClassName + ".generated.cs");
                using (var sourceWriter = new StreamWriter(apiClassFileName))
                {
                    provider.GenerateCodeFromCompileUnit(ApiCompileUnit, sourceWriter, options);
                }
                File.WriteAllText(apiClassFileName, File.ReadAllText(apiClassFileName, Encoding.UTF8).Replace(" partial class ", " unsafe partial class "));
            }
        }

        private static (CodeNamespace Namespace, CodeTypeDeclaration GeneratedClass) DeclareClass(CodeCompileUnit compileUnit, string name, params string[] usings)
        {
            var codeNamespace = new CodeNamespace("CoreLoader.OpenGL");
            foreach (var import in usings)
            {
                codeNamespace.Imports.Add(new CodeNamespaceImport(import));
            }
            compileUnit.Namespaces.Add(codeNamespace);
            var generatedClass = new CodeTypeDeclaration(name)
            {
                IsPartial = true
            };
            codeNamespace.Types.Add(generatedClass);
            return (codeNamespace, generatedClass);
        }
    }
}