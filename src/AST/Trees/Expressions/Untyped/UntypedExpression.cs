using Common;

namespace AST.Trees.Expressions.Untyped
{
    public abstract class UntypedExpression : Expression
    {
        protected UntypedExpression(DiagnosticInfo diag, UntypedScopeContext context) : base(diag, context)
        {
        }

        public abstract override string ToString();
    }
}
