using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantMathUntypedExpression : UntypedExpression
    {
        public ConstantMathUntypedExpression(DiagnosticInfo diag, object value, ScopeContext context) : base(diag, context)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override string ToString()
        {
            return $"ConstantUntypedMath: {Value}";
        }
    }
}
