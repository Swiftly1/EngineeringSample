using Common;

namespace AST.Trees.Expressions
{
    public abstract class Expression : Node
    {
        protected Expression(DiagnosticInfo diag) : base(diag)
        {
        }

        public abstract override string ToString();
    }
}
