using Common;

namespace AST.Trees
{
    public abstract class ScopeableNode : Node
    {
        protected ScopeableNode(DiagnosticInfo diag, ScopeContext parent_context) : base(diag)
        {
            ScopeContext = parent_context;
        }

        protected ScopeableNode(DiagnosticInfo diag, string nameSpace) : base(diag)
        {
            ScopeContext.Name = nameSpace;
        }

        public ScopeContext ScopeContext { get; } = new();
    }
}