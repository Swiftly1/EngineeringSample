using System;
using Common;

namespace AST.Miscs
{
    public class ASTException : Exception
    {
        public DiagnosticInfo DiagnosticInfo { get; set; }

        public ASTException(string message, DiagnosticInfo diag) : base(message)
        {
            DiagnosticInfo = diag;
        }

        private ASTException() : base()
        {
        }

        public ASTException(string message) : base(message)
        {
        }

        public ASTException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
