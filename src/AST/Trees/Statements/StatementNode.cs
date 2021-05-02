using Common;

namespace AST.Trees.Statements
{
    public abstract class StatementNode : Node
    {
        protected StatementNode(DiagnosticInfo diag) : base(diag)
        {
        }

        public abstract override string ToString();
    }
}
