using Common;

namespace AST.Trees
{
    public class RootNode : Node
    {
        public RootNode(DiagnosticInfo diag) : base(diag)
        {
        }

        public override string ToString()
        {
            return "Root";
        }
    }
}
