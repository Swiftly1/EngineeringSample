using Common;
using AST.Miscs;
using System.Linq;
using Common.Lexing;
using System.Collections.Generic;
using AST.Trees.Expressions.Untyped;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Expressions
{
    public class ExpressionBuilder : Movable<LexElement>
    {
        public ExpressionBuilder(List<LexElement> expressionElements) : base(expressionElements)
        {
        }

        public ResultDiag<UntypedExpression> Build()
        {
            try
            {
                var left = GetSubExpression();

                if (!CanMoveNext())
                    return new ResultDiag<UntypedExpression>(left);

                var @operator = _Current as LexCharacter;
                var higher_prior_operators = new[] { LexingElement.Plus, LexingElement.Minus };

                while (higher_prior_operators.Contains(@operator.Kind))
                {
                    MoveNext();
                    var right = GetSubExpression();
                    left = new ComplexUntypedExpression(left, right, OperatorFacts.Convert(@operator.Kind), left.Diagnostics);

                    if (MoveNext())
                    {
                        @operator = _Current as LexCharacter;
                    }

                    break;
                }

                return new ResultDiag<UntypedExpression>(left);
            }
            catch (ASTException ex)
            {
                return new ResultDiag<UntypedExpression>(ex.Message, ex.DiagnosticInfo);
            }
        }

        private UntypedExpression GetSubExpression()
        {
            var left = ToExpression(_Current);

            if (!MoveNext())
                return left;

            var @operator = _Current as LexCharacter;
            var higher_prior_operators = new[] { LexingElement.Star, LexingElement.Slash };

            while (higher_prior_operators.Contains(@operator.Kind))
            {
                MoveNext();
                var right = GetSubExpression();
                left = new ComplexUntypedExpression(left, right, OperatorFacts.Convert(@operator.Kind), left.Diagnostics);

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
            {
                var numerical = left as LexNumericalLiteral;
                return new ConstantMathUntypedExpression(left.Diagnostics, numerical.StringValue);
            }
            else if (left.Kind == LexingElement.Word)
                return new UntypedVariableUseExpression(left.Diagnostics, (left as LexWord).Value);

            throw new ASTException($"Unable to resolve Expression for kind {left.Kind}. Location: {left.Diagnostics}", left.Diagnostics);
        }
    }
}
