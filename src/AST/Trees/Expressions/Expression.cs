using Common;

namespace AST.Trees.Expressions
{
    public abstract class Expression : Node
    {
        protected Expression(DiagnosticInfo diag, UntypedScopeContext context) : base(diag)
        {
            ScopeContext = context;
        }

        public UntypedScopeContext ScopeContext { get; }

        public abstract override string ToString();
    }
}
