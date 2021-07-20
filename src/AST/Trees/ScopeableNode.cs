using Common;

namespace AST.Trees
{
    public abstract class ScopeableNode : Node
    {
        protected ScopeableNode(DiagnosticInfo diag, ScopeContext context) : base(diag)
        {
            ScopeContext.Parent = context;
        }

        public ScopeContext ScopeContext { get; set; } = new();
    }
}