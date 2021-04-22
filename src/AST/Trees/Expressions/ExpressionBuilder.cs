using Common;
using System.Collections.Generic;
using AST.Trees.Expressions.Untyped;
using Text2Abstraction.LexicalElements;
using Common.Lexing;
using AST.Miscs;

namespace AST.Trees.Expressions
{
    public class ExpressionBuilder : Movable<LexElement>
    {
        public ExpressionBuilder(List<LexElement> expressionElements) : base(expressionElements)
        {
        }

        public Result<UntypedExpression> Build()
        {
            try
            {
                var tree = GetOperand();
                return new Result<UntypedExpression>(tree);
            }
            catch (ASTException ex)
            {
                return new Result<UntypedExpression>(ex.Message, ex.DiagnosticInfo);
            }
        }

        private UntypedExpression GetOperand()
        {
            var left = ToExpression(_Current);

            if (!MoveNext())
                return left;

            var @operator = _Current as LexCharacter;

            while (@operator.Kind == LexingElement.Star)
            {
                MoveNext();
                var right = GetOperand();
                left = new ComplexUntypedExpression(left, right, @operator, left.Diagnostics);

                if (MoveNext())
                {
                    @operator = _Current as LexCharacter;
                }
                else
                {
                    return left;
                }
            }

            return left;
        }

        private UntypedExpression ToExpression(LexElement left)
        {
            if (left.Kind == LexingElement.Numerical)
                return new ConstantMathUntypedExpression(left.Diagnostics, (left as LexNumericalLiteral).GetNumericalValue());

            throw new ASTException($"Unable to resolve Expression for kind {left.Kind}. Location: {left.Diagnostics}", left.Diagnostics);
        }
    }
}
