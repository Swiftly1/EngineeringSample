using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantUntypedStringExpression : UntypedExpression
    {
        public ConstantUntypedStringExpression(DiagnosticInfo diag, string value) : base(diag)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"ConstantUntypedString: {Value}";
        }
    }
}
