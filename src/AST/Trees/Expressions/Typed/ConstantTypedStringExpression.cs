using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantTypedStringExpression : TypedExpression
    {
        public ConstantTypedStringExpression(DiagnosticInfo diag, string value, TypeInfo typeInfo) : base(diag, typeInfo)
        {
            Value = value;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"ConstantTypedString: {Value}";
        }
    }
}
