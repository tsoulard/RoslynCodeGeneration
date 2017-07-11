using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;
using SyntaxGeneration.Models;

namespace SyntaxGeneration.Builder
{
    public class CodeGenerator
    {
        private readonly SyntaxGenerator _syntaxGenerator;

        public CodeGenerator(SyntaxGenerator syntaxGenerator)
        {
            _syntaxGenerator = syntaxGenerator;
        }

        public SyntaxNode LinqQuery(string variableName, string linqName, string identifierName, params SyntaxNode[] syntaxNodes)
        {
            var lambda = _syntaxGenerator.VoidReturningLambdaExpression(identifierName, syntaxNodes);

            var method = AccessMethod(variableName, linqName, lambda);

            return method;
        }

        //Class Expressions
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

        public SyntaxNode ClassFieldGeneration(string fieldName, string className, Accessibility type, DeclarationModifiers declarationModifier)
        {
            return _syntaxGenerator.FieldDeclaration(fieldName, _syntaxGenerator.IdentifierName(className), type,
                declarationModifier);
        }

        public SyntaxNode[] AddUsingStatements(params string[] usingStatements)
        {
            var synataxNodes = new List<SyntaxNode>();
            foreach (var usingStatement in usingStatements)
            {
                var namespaceImport = _syntaxGenerator.NamespaceImportDeclaration(usingStatement);
                synataxNodes.Add(namespaceImport);
            }
            return synataxNodes.ToArray();
        }

        //Method Expressions
        public SyntaxNode AccessMethod(string variableName, string methodName, params SyntaxNode[] arguements)
        {
            var method = _syntaxGenerator.MemberAccessExpression(_syntaxGenerator.IdentifierName(variableName),
                _syntaxGenerator.IdentifierName(methodName));

            return _syntaxGenerator.InvocationExpression(method, arguements);
        }

        public SyntaxNode MethodDecleration(string methodName, SyntaxNode[] parameters, string returnType,
            Accessibility type, DeclarationModifiers declarationModifier = default(DeclarationModifiers),
            params SyntaxNode[] statements)
        {
            var returnTypeNode = string.IsNullOrEmpty(returnType) ? null : _syntaxGenerator.IdentifierName(returnType);

            var method = _syntaxGenerator.MethodDeclaration(methodName, parameters, null, returnTypeNode, type,
                declarationModifier, statements);

            return method;
        }

        public SyntaxNode ReturnStatement(string variableName)
        {
            return _syntaxGenerator.ReturnStatement(_syntaxGenerator.IdentifierName(variableName));
        }

        //Variable Expressions
        public SyntaxNode[] CreateArguements(params string[] arguementNames)
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
    }
}