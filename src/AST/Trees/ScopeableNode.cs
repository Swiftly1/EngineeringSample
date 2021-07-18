using Common;

namespace AST.Trees
{
    public abstract class ScopeableNode : Node
    {
        protected ScopeableNode(DiagnosticInfo diag, ScopeContext context) : base(diag)
        {
            Context.Parent = context;
        }

        public ScopeContext Context { get; set; } = new();
    }
}