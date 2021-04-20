using Common;
using System.Collections.Generic;
using AST.Trees.Expressions.Untyped;
using Text2Abstraction.LexicalElements;
using System;
using Common.Lexing;

namespace AST.Trees.Expressions
{
    public class ExpressionBuilder : Movable<LexElement>
    {
        public ExpressionBuilder(List<LexElement> expressionElements) : base(expressionElements)
        {
        }

        public Result<UntypedExpression> Build()
        {
            var tree = GetOperand();
            return new Result<UntypedExpression>(tree);
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

            throw new NotImplementedException($"Unable to resolve Expression for kind {left.Kind}. Location: {left.Diagnostics}");
        }
    }
}
