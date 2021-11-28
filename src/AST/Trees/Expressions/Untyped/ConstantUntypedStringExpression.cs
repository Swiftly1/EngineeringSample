using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantUntypedStringExpression : UntypedExpression
    {
        public ConstantUntypedStringExpression(DiagnosticInfo diag, string value, ScopeContext context) : base(diag, context)
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
