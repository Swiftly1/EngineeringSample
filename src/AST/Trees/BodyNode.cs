using Common;

namespace AST.Trees
{
    public class BodyNode : ScopeableNode
    {
        public BodyNode(DiagnosticInfo diag, ScopeContext sc) : base(diag, sc)
        {
        }

        public override string ToString()
        {
            return $"Body";
        }
    }
}
