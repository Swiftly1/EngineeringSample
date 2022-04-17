using Common;
using AST.Miscs;
using System.Linq;
using Common.Lexing;
using AST.Trees.Miscs;
using System.Collections.Generic;
using AST.Trees.Expressions.Untyped;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Expressions
{
    public class ExpressionBuilder : MovableLookup
    {
        public ExpressionBuilder(List<LexElement> expressionElements, UntypedScopeContext context) : base(expressionElements)
        {
            ScopeContext = context;
        }

        public UntypedScopeContext ScopeContext { get; }

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
                    left = new ComplexUntypedExpression(left, right, OperatorFacts.Convert(@operator.Kind), left.Diagnostics, ScopeContext);

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
            var higher_prior_operators = new[] { LexingElement.Star, LexingElement.Slash, LexingElement.GreaterOrEqual, 
                LexingElement.GreaterThan, LexingElement.LessOrEqual, LexingElement.LessThan };

            while (higher_prior_operators.Contains(@operator.Kind))
            {
                MoveNext();
                var right = GetSubExpression();
                left = new ComplexUntypedExpression(left, right, OperatorFacts.Convert(@operator.Kind), left.Diagnostics, ScopeContext);

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
                return new ConstantMathUntypedExpression(left.Diagnostics, numerical.StringValue, ScopeContext);
            }
            else if (left.Kind == LexingElement.Word)
            {
                var ahead = TryGetAhead(1, movePointer: false);

                if (ahead.Sucess && ahead.Items[0].Kind == LexingElement.OpenParenthesis)
                {
                    // function call e.g 2 + test(expression, expression...)
                    MoveNext();
                    var result = GetTillClosed(LexingElement.OpenParenthesis, LexingElement.ClosedParenthesis);
                    var args = FunctionHelpers.ExtractFunctionCallParameters(result.Data, ScopeContext);

                    if (!args.Success)
                        throw new ASTException(args.Message, left.Diagnostics);

                    return new UntypedFunctionCallExpression(left.Diagnostics, (left as LexWord).Value, args.Data, ScopeContext);
                }
                else
                {
                    // using variable e.g 2 + a
                    return new UntypedVariableUseExpression(left.Diagnostics, (left as LexWord).Value, ScopeContext);
                }
            }
            else if (left.Kind == LexingElement.String)
                return new ConstantUntypedStringExpression(left.Diagnostics, (left as LexStringLiteral).Value, ScopeContext);

            throw new ASTException($"Unable to resolve Expression for kind {left.Kind}. Location: {left.Diagnostics}", left.Diagnostics);
        }
    }
}
