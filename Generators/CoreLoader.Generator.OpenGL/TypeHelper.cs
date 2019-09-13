using System;
using System.CodeDom;
using System.Linq;
using System.Text;

namespace CoreLoader.Generator.OpenGL
{
    public static class TypeHelper
    {
        public static Type GetType(string typeName, out EnumType enumType, bool validatedName = false)
        {
            enumType = EnumType.None;
            if (typeName.StartsWith("GLchar **") || typeName.StartsWith("char **") || typeName.StartsWith("const GLchar **") || typeName.StartsWith("const char **"))
                return typeof(string).MakeArrayType();
            if (typeName.StartsWith("GLchar *") || typeName.StartsWith("char *"))
                return typeof(StringBuilder);
            if (typeName.StartsWith("const GLchar *") || typeName.StartsWith("const char *"))
                return typeof(string);

            if (!validatedName)
            {
                bool ptr;
                (typeName, ptr) = ParseTypeName(typeName);
                if (ptr)
                    return GetType(typeName, out enumType, true).MakePointerType();
            }

            switch (typeName.ToLower())
            {
                case "void":
                case "glvoid":
                    return typeof(void);
                case "glboolean":
                    return typeof(bool);
                case "glbyte":
                    return typeof(sbyte);
                case "glubyte":
                    return typeof(byte);
                case "glshort":
                    return typeof(short);
                case "glushort":
                    return typeof(ushort);
                case "glint":
                case "glsizei":
                    return typeof(int);
                case "glenum":
                    enumType = EnumType.Enum;
                    return typeof(uint);
                case "glbitfield":
                    enumType = EnumType.Bitfield;
                    return typeof(uint);
                case "gluint":
                    return typeof(uint);
                case "glint64":
                case "glsizeiptr":
                case "glintptr":
                    return typeof(long);
                case "gluint64":
                    return typeof(ulong);
                case "glfloat":
                    return typeof(float);
                case "gldouble":
                    return typeof(double);
                case "glsync":
                    return typeof(IntPtr);
                case "debugproc":
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeName), $"No .Net type defined for {typeName}");
            }
        }

        private static (string Name, bool IsPtr) ParseTypeName(string typeName)
        {
            var index = typeName.IndexOf(' ');
            if (index == -1)
                throw new ArgumentException($"Type {typeName} is not a valid type", nameof(typeName));

            var type = typeName.Substring(0, index);
            if (type == "const")
            {
                const int constStart = 6;
                return ParseTypeName(typeName.Substring(constStart));
            }

            var isPtr = typeName.Contains('*');
            return (type, isPtr);
        }

        public static CodeTypeReference GetReturnType(Type returnType)
        {
            if (returnType.IsPointer)
            {
                var pointerCount = returnType.FullName.Count(c => c == '*');
                return new CodeTypeReference(GetSimpleTypeName(returnType.FullName.Replace("*", "")) + new string('*', pointerCount));
            }
            return new CodeTypeReference(returnType);
        }

        public static CodeParameterDeclarationExpression GetParameterType(Type type, string typeName, string name)
        {
            if (type == null)
            {
                var paramTypeName = typeName.Replace("const ", "").Split(' ').First();
                if (paramTypeName == "DEBUGPROC")
                    paramTypeName = "DebugProc";

                return new CodeParameterDeclarationExpression(paramTypeName, name);
            }

            if (type.IsByRef)
                return new CodeParameterDeclarationExpression($"ref {GetSimpleTypeName(type.FullName.Replace("&", ""))}", name);

            if (type.IsPointer)
            {
                var pointerCount = type.FullName.Count(c => c == '*');
                return new CodeParameterDeclarationExpression(GetSimpleTypeName(type.FullName.Replace("*", "")) + new string('*', pointerCount), name);
            }
            return new CodeParameterDeclarationExpression(type, name);
        }

        private static string GetSimpleTypeName(string typeName)
        {
            switch (typeName)
            {
                case "System.Void":
                    return "void";
                case "System.String":
                    return "string";
                case "System.Boolean":
                    return "bool";
                case "System.Byte":
                    return "byte";
                case "System.SByte":
                    return "sbyte";
                case "System.Int16":
                    return "short";
                case "System.UInt16":
                    return "ushort";
                case "System.Int32":
                    return "int";
                case "System.UInt32":
                    return "uint";
                case "System.Int64":
                    return "long";
                case "System.UInt64":
                    return "ulong";
                case "System.Single":
                    return "float";
                case "System.Double":
                    return "double";
                default:
                    return typeName;
            }
        }
    }
}