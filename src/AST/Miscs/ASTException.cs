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

        private ASTException(string message) : base(message)
        {
        }

        private ASTException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
