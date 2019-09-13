using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CoreLoader.Generator.OpenGL
{
    public class CodeAnalyzer
    {
        private readonly ClassDeclarationSyntax _classNode;

        public CodeAnalyzer(string fileContent)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(fileContent);
            var root = syntaxTree.GetCompilationUnitRoot();
            _classNode = GetClassNode(root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDefinition">Format: Type.Method</param>
        /// <returns></returns>
        public bool HasCallTo(string methodDefinition)
        {
            if (_classNode == null)
                return false;

            foreach (var method in _classNode.Members.OfType<MethodDeclarationSyntax>())
            {
                if (method.ExpressionBody != null && method.ExpressionBody.Expression is InvocationExpressionSyntax invocationExpression && invocationExpression.Expression.ToString() == methodDefinition)
                    return true;

                if (method.Body != null && method.Body.Statements.Any(statement => statement.ToString().Contains(methodDefinition + "(")))
                    return true;
            }
            return false;
        }

        public bool HasField(string fieldName)
        {
            return _classNode.Members
                .OfType<FieldDeclarationSyntax>()
                .Any(field => field.Declaration.Variables.ToString() == fieldName);
        }

        private ClassDeclarationSyntax GetClassNode(CompilationUnitSyntax root)
        {
            if (root.Members.First() is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                return namespaceDeclaration.Members
                    .OfType<ClassDeclarationSyntax>()
                    .First(c => c.Identifier.ToString() == "GL" || c.Identifier.ToString() == "GlNative");
            }
            return null;
        }
    }
}