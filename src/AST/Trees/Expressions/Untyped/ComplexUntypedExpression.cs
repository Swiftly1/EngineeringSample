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
            Left = left;
            Operator = @operator;
            Right = right;

            Children.Add(Left);
            Children.Add(Right);
        }

        public UntypedExpression Left { get; set; }

        public ExpressionOperator Operator { get; set; }

        public UntypedExpression Right { get; set; }

        public override string ToString()
        {
            return $"Complex Untyped Expression: Operator: '{Enum.GetName(typeof(ExpressionOperator), Operator)}'";
        }
    }
}
