using Common;

namespace AST.Trees
{
    public class BodyNode : ScopeableNode
    {
        public BodyNode(DiagnosticInfo diag, UntypedScopeContext sc) : base(diag, sc)
        {
        }

        public override string ToString()
        {
            return "Body";
        }
    }
}
