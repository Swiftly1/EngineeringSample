using Common;

namespace AST.Trees
{
    public abstract class ScopeableNode : Node
    {
        protected ScopeableNode(DiagnosticInfo diag, ScopeContext parent_context) : base(diag)
        {
            ScopeContext.Parent = parent_context;
        }

        protected ScopeableNode(DiagnosticInfo diag, string nameSpace) : base(diag)
        {
            ScopeContext.Namespace = nameSpace;
        }

        public ScopeContext ScopeContext { get; } = new();
    }
}