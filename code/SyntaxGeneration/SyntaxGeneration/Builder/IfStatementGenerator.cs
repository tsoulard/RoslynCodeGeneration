using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Editing;

namespace SyntaxGeneration.Builder
{
    public class IfStatementGenerator
    {
        private readonly SyntaxGenerator _syntaxGenerator;

        public IfStatementGenerator(SyntaxGenerator syntaxGenerator)
        {
            _syntaxGenerator = syntaxGenerator;
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
    }
}