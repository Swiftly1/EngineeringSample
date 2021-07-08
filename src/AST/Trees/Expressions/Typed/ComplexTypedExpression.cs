using Common;

namespace AST.Trees.Expressions.Typed
{
    public class ComplexTypedExpression : TypedExpression
    {
        public ComplexTypedExpression(
            TypedExpression left,
            TypedExpression right,
            ExpressionOperator @operator,
            TypeInfo typeInfo,
            DiagnosticInfo diag) : base(diag, typeInfo)
        {
            Left = left;
            Right = right;
            Operator = @operator;
            this.Children.Add(Left);
            this.Children.Add(Right);
        }

        public TypedExpression Left { get; set; }

        public ExpressionOperator Operator { get; set; }

        public TypedExpression Right { get; set; }

        public override string ToString()
        {
            return $"ComplexTyped: {TypeInfo.Name}; Operator: {Operator}";
        }
    }
}
