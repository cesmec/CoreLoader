using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreLoader.Generator.OpenGL
{
    public class Program
    {
        public static async Task Main()
        {
            var projectPath = Path.GetFullPath(Path.Combine("..", "..", ".."));
            var externalsPath = Path.Combine(projectPath, "externals");
            var bindingsProjectPath = Path.GetFullPath(Path.Combine(projectPath, "..", "..", "CoreLoader.OpenGL"));
            var gl4Docs = Path.Combine(projectPath, "OpenGL-Refpages", "gl4");

            var constRegex = new Regex(@"^#define (GL_\S+)\s+((0x)?[0-9a-fA-F]+)$");
            var nativeConstants = (await File.ReadAllLinesAsync(Path.Combine(externalsPath, "glcorearb.h")))
                .Where(line => line.StartsWith("#define GL_"))
                .Select(line => constRegex.Match(line))
                .Where(match => match.Success)
                .ToDictionary(match => match.Groups[1].Value, match => Convert.ToUInt32(match.Groups[2].Value, match.Groups[3].Success ? 16 : 10));

            CodeAnalyzer codeAnalyzer;
            using (var glFileStream = File.Open(Path.Combine(bindingsProjectPath, "GL.cs"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var glFileReader = new StreamReader(glFileStream))
            {
                codeAnalyzer = new CodeAnalyzer(await glFileReader.ReadToEndAsync());
            }

            CodeAnalyzer nativeCodeAnalyzer;
            using (var nativeGlFileStream = File.Open(Path.Combine(bindingsProjectPath, "GlNative.cs"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var nativeGlFileReader = new StreamReader(nativeGlFileStream))
            {
                nativeCodeAnalyzer = new CodeAnalyzer(await nativeGlFileReader.ReadToEndAsync());
            }

            var codeUnit = new BindingCodeUnit();
            var enumGenerator = new EnumGenerator(codeUnit, nativeConstants);
            GenerateConstClass(codeUnit, nativeConstants);

            //currently only migrating gl*
            foreach (var file in Directory.EnumerateFiles(gl4Docs, "gl*.xml"))
            {
                if (file.StartsWith("gl_"))
                    continue;

                try
                {
                    var generator = new BindingCodeGenerator(file);
                    await generator.GenerateCodeAsync(codeUnit, codeAnalyzer, nativeCodeAnalyzer, enumGenerator);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to generate code for {Path.GetFileName(file)}: " + e);
                }
            }

            codeUnit.GenerateClasses(bindingsProjectPath);
        }

        private static void GenerateConstClass(BindingCodeUnit codeUnit, Dictionary<string, uint> constants)
        {
            var constClass = new CodeTypeDeclaration("GlConsts");

            foreach (var constant in constants)
            {
                constClass.Members.Add(new CodeMemberField(typeof(uint), constant.Key)
                {
                    Attributes = MemberAttributes.Public | MemberAttributes.Const,
                    InitExpression = new CodePrimitiveExpression(constant.Value)
                });
            }

            codeUnit.ApiGeneratedNamespace.Types.Add(constClass);
        }
    }
}