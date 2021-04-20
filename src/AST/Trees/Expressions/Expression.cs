using Common;

namespace AST.Trees.Expressions
{
    public abstract class Expression
    {
        public Expression(DiagnosticInfo diag)
        {
            Diagnostics = diag;
        }

        public DiagnosticInfo Diagnostics { get; }

        public abstract override string ToString();
    }
}
