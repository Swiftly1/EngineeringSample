using Common;

namespace AST.Trees
{
    public class BodyNode : ScopeableNode
    {
        public BodyNode(DiagnosticInfo diag) : base(diag)
        {
        }

        public override string ToString()
        {
            return $"Body";
        }
    }
}
