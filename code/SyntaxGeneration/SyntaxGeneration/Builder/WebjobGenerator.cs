using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace SyntaxGeneration.Builder
{
    public class WebjobGenerator
    {
        private readonly SyntaxGenerator _syntaxGenerator;

        public WebjobGenerator()
        {
            var workspace = new AdhocWorkspace();
            _syntaxGenerator = SyntaxGenerator.GetGenerator(workspace, LanguageNames.CSharp);
        }

        public SyntaxNode[] AddUsingStatements(string[] usingStatements)
        {
            var synataxNodes = new List<SyntaxNode>();
            foreach (var usingStatement in usingStatements)
            {
                var namespaceImport = _syntaxGenerator.NamespaceImportDeclaration(usingStatement);
                synataxNodes.Add(namespaceImport);
            }
            return synataxNodes.ToArray();
        }

        public SyntaxNode ClassFieldGeneration(string fieldName, string className, Accessibility type, DeclarationModifiers declarationModifier)
        {

            _syntaxGenerator

            return _syntaxGenerator.FieldDeclaration(fieldName, _syntaxGenerator.IdentifierName(className), type,
                declarationModifier);
        }


        public SyntaxNode ConstructorGeneration(string className, List<ConstructorAssignmentModel> constructorAssignmentModels, Accessibility accessibilityType, DeclarationModifiers declaration)
        {
            if (!constructorAssignmentModels.Any())
            {
                return null;
            }

            var construcorAssignmnetsNodes = new List<SyntaxNode>();
            var construcorParameterDeclarationNodes = new List<SyntaxNode>();
            foreach (var construcorAssignmnet in constructorAssignmentModels)
            {
                var assignmentStatement = _syntaxGenerator.AssignmentStatement(
                    _syntaxGenerator.IdentifierName(construcorAssignmnet.GlobalVariableName),
                    _syntaxGenerator.IdentifierName(construcorAssignmnet.VariableName));

                construcorAssignmnetsNodes.Add(assignmentStatement);

                var parameterDeclaration = ParameterGeneration(construcorAssignmnet.VariableName, construcorAssignmnet.ClassName);

                construcorParameterDeclarationNodes.Add(parameterDeclaration);
            }

            var constructor = _syntaxGenerator.ConstructorDeclaration(className,
                construcorParameterDeclarationNodes, accessibilityType,
                declaration, null, construcorAssignmnetsNodes);

            return constructor;
        }

        public SyntaxNode AccessMethod(string variableName, string methodName, SyntaxNode[] arguements)
        {
            var method = _syntaxGenerator.MemberAccessExpression(_syntaxGenerator.IdentifierName(variableName),
                _syntaxGenerator.IdentifierName(methodName));

            return _syntaxGenerator.InvocationExpression(method, arguements);
        }

        public SyntaxNode[] CreateArguements(string[] arguementNames)
        {
            var syntaxNodes = new List<SyntaxNode>();
            foreach (var arguementName in arguementNames)
            {
                var arguement = _syntaxGenerator.Argument(RefKind.None, _syntaxGenerator.IdentifierName(arguementName));
                syntaxNodes.Add(arguement);
            }

            return syntaxNodes.ToArray();
        }

        public SyntaxNode CreateVariable(string variableName, SyntaxNode node)
        {
            return _syntaxGenerator.LocalDeclarationStatement(variableName, node);
        }

        public SyntaxNode AddissionExpression(string valueOne, string valueTwo)
        {
            return _syntaxGenerator.AddExpression(_syntaxGenerator.IdentifierName(valueOne),
                _syntaxGenerator.IdentifierName(valueTwo));
        }
        
        public SyntaxNode ParameterGeneration(string parameterName, string className)
        {
            return _syntaxGenerator.ParameterDeclaration(parameterName, _syntaxGenerator.IdentifierName(className));
        }
        public SyntaxNode ParameterGeneration(string parameterName, SpecialType type)
        {
            return _syntaxGenerator.ParameterDeclaration(parameterName, _syntaxGenerator.TypeExpression(type));
        }
    }

    public class ConstructorAssignmentModel
    {
        public string ClassName { get; set; }
        public string VariableName { get; set; }
        public string GlobalVariableName { get; set; }
    }
}