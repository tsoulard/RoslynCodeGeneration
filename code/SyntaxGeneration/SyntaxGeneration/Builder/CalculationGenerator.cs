using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace SyntaxGeneration.Builder
{
    public class CalculationGenerator
    {
        private readonly SyntaxGenerator _syntaxGenerator;
        
        public CalculationGenerator(SyntaxGenerator syntaxGenerator)
        {
            _syntaxGenerator = syntaxGenerator;
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
}