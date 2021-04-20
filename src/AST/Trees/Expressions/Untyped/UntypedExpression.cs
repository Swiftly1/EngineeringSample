using Common;

namespace AST.Trees.Expressions.Untyped
{
    public abstract class UntypedExpression : Expression
    {
        public UntypedExpression(DiagnosticInfo diag) : base(diag)
        {
        }

        public abstract override string ToString();
    }
}
