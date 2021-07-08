using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantMathUntypedExpression : UntypedExpression
    {
        public ConstantMathUntypedExpression(DiagnosticInfo diag, object value) : base(diag)
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
