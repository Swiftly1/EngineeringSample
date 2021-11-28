using Common;

namespace AST.Trees.Expressions
{
    public abstract class Expression : Node
    {
        protected Expression(DiagnosticInfo diag, ScopeContext context) : base(diag)
        {
            Context = context;
        }

        public ScopeContext Context { get; }

        public abstract override string ToString();
    }
}
