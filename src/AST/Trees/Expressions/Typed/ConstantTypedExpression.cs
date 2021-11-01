using Common;

namespace AST.Trees.Expressions.Typed
{
    public class ConstantTypedExpression : TypedExpression
    {
        public ConstantTypedExpression(DiagnosticInfo diag, object value, TypeInfo typeInfo) : base(diag, typeInfo)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override bool IsConstant()
        {
            return true;
        }

        public override string ToString()
        {
            return $"ConstantTyped: {TypeInfo.Name}: {Value}";
        }
    }
}
