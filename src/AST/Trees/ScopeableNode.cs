using Common;

namespace AST.Trees
{
    public abstract class ScopeableNode : Node
    {
        public ScopeableNode(DiagnosticInfo diag) : base(diag)
        {
        }
    }
}