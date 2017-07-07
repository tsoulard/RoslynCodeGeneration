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

        public WebjobGenerator(SyntaxGenerator syntaxGenerator)
        {
            _syntaxGenerator = syntaxGenerator;
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

        public SyntaxNode ClassFieldGeneration(string fieldName, string className, Accessibility type, DeclarationModifiers declarationModifier)
        {
            return _syntaxGenerator.FieldDeclaration(fieldName, _syntaxGenerator.IdentifierName(className), type,
                declarationModifier);
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

        public SyntaxNode LinqQuery(string variableName, string linqName, string identifierName, params SyntaxNode[] syntaxNodes)
        {
            var lambda = _syntaxGenerator.VoidReturningLambdaExpression(identifierName, syntaxNodes);

            var method = AccessMethod(variableName, linqName, lambda);

            return method;
        }

        public SyntaxNode IfEqualsStatement(string variableOne, string variableTwo, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.ValueEqualsExpression(_syntaxGenerator.IdentifierName(variableOne), _syntaxGenerator.IdentifierName(variableTwo));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
        }

        public SyntaxNode IfEqualsStatement(string variable, object item, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.ValueEqualsExpression(_syntaxGenerator.IdentifierName(variable), _syntaxGenerator.LiteralExpression(item));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
        }

        public SyntaxNode IfGreaterThanExpression(string variableOne, string variableTwo, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.GreaterThanExpression(_syntaxGenerator.IdentifierName(variableOne), _syntaxGenerator.IdentifierName(variableTwo));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
        }

        public SyntaxNode IfGreaterThanExpression(string variable, object item, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.GreaterThanExpression(_syntaxGenerator.IdentifierName(variable), _syntaxGenerator.LiteralExpression(item));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
        }

        public SyntaxNode IfLessThanExpression(string variableOne, string variableTwo, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.LessThanExpression(_syntaxGenerator.IdentifierName(variableOne), _syntaxGenerator.IdentifierName(variableTwo));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
        }

        public SyntaxNode IfLessThanExpression(string variable, object item, SyntaxNode[] trueBlocks, SyntaxNode[] falseBlocks)
        {
            var condition = _syntaxGenerator.LessThanExpression(_syntaxGenerator.IdentifierName(variable), _syntaxGenerator.LiteralExpression(item));
            var ifStatement = _syntaxGenerator.IfStatement(condition, trueBlocks, falseBlocks);

            return ifStatement;
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

        public SyntaxNode AccessMethod(string variableName, string methodName, params SyntaxNode[] arguements)
        {
            var method = _syntaxGenerator.MemberAccessExpression(_syntaxGenerator.IdentifierName(variableName),
                _syntaxGenerator.IdentifierName(methodName));

            return _syntaxGenerator.InvocationExpression(method, arguements);
        }

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

        public SyntaxNode AddissionExpression(string valueOne, string valueTwo)
        {
            return _syntaxGenerator.AddExpression(_syntaxGenerator.IdentifierName(valueOne),
                _syntaxGenerator.IdentifierName(valueTwo));
        }

        public SyntaxNode AddissionExpression(object valueOne, object valueTwo)
        {
            return _syntaxGenerator.AddExpression(_syntaxGenerator.LiteralExpression(valueOne),
                _syntaxGenerator.LiteralExpression(valueTwo));
        }

        public SyntaxNode SubtractExpression(string valueOne, string valueTwo)
        {
            return _syntaxGenerator.SubtractExpression(_syntaxGenerator.IdentifierName(valueOne),
                _syntaxGenerator.IdentifierName(valueTwo));
        }

        public SyntaxNode SubtractExpression(object valueOne, object valueTwo)
        {
            return _syntaxGenerator.SubtractExpression(_syntaxGenerator.LiteralExpression(valueOne),
                _syntaxGenerator.LiteralExpression(valueTwo));
        }

        public SyntaxNode MultiplicationExpression(string valueOne, string valueTwo)
        {
            return _syntaxGenerator.MultiplyExpression(_syntaxGenerator.IdentifierName(valueOne),
                _syntaxGenerator.IdentifierName(valueTwo));
        }

        public SyntaxNode MultiplicationExpression(object valueOne, object valueTwo)
        {
            return _syntaxGenerator.MultiplyExpression(_syntaxGenerator.LiteralExpression(valueOne),
                _syntaxGenerator.LiteralExpression(valueTwo));
        }

        public SyntaxNode DivideExpression(string valueOne, string valueTwo)
        {
            return _syntaxGenerator.DivideExpression(_syntaxGenerator.IdentifierName(valueOne),
                _syntaxGenerator.IdentifierName(valueTwo));
        }

        public SyntaxNode DivideExpression(object valueOne, object valueTwo)
        {
            return _syntaxGenerator.DivideExpression(_syntaxGenerator.LiteralExpression(valueOne),
                _syntaxGenerator.LiteralExpression(valueTwo));
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