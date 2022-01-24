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
            DiagnosticInfo diag,
            UntypedScopeContext context) : base(diag, typeInfo, context)
        {
            Operator = @operator;
            this.Children.Add(left);
            this.Children.Add(right);
        }

        public TypedExpression Left => Children[0] as TypedExpression;

        public ExpressionOperator Operator { get; set; }

        public TypedExpression Right => Children[1] as TypedExpression;

        public override bool IsConstant()
        {
            return Left.IsConstant() && Right.IsConstant();
        }

        public override string ToString()
        {
            return $"ComplexTyped: {TypeInfo.Name}; Operator: {Operator}";
        }
    }
}
