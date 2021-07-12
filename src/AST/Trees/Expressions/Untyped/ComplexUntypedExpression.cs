using System;
using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class ComplexUntypedExpression : UntypedExpression
    {
        public ComplexUntypedExpression(
            UntypedExpression left,
            UntypedExpression right,
            ExpressionOperator @operator,
            DiagnosticInfo diag) : base(diag)
        {
            Operator = @operator;

            Children.Add(left);
            Children.Add(right);
        }

        public UntypedExpression Left => Children[0] as UntypedExpression;

        public ExpressionOperator Operator { get; set; }

        public UntypedExpression Right => Children[1] as UntypedExpression;

        public override string ToString()
        {
            return $"Complex Untyped Expression: Operator: '{Enum.GetName(typeof(ExpressionOperator), Operator)}'";
        }
    }
}
