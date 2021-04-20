using Common;

namespace AST.Trees.Expressions.Typed
{
    public class ComplexTypedExpression : TypedExpression
    {
        public ComplexTypedExpression(DiagnosticInfo diag) : base(diag)
        {
        }

        public TypedExpression Left { get; set; }

        public TypedExpression Right { get; set; }

        public override string ToString()
        {
            return $"Complex Typed: Left:{Left}; Right:{Right}";
        }
    }
}
