using Common;

namespace AST.Trees.Expressions.Typed
{
    public abstract class TypedExpression : Expression
    {
        protected TypedExpression(DiagnosticInfo diag) : base(diag)
        {
        }

        public TypeInfo TypeInfo { get; set; }

        public abstract override string ToString();
    }
}
