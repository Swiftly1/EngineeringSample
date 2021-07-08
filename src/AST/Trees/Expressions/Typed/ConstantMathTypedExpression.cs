using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Expressions.Untyped
{
    public class ConstantMathTypedExpression : TypedExpression
    {
        public ConstantMathTypedExpression(DiagnosticInfo diag, object value, TypeInfo typeInfo) : base(diag, typeInfo)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override string ToString()
        {
            return $"ConstantTypedMath: {TypeInfo.Name}: {Value}";
        }
    }
}
