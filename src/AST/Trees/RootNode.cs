using System;
using Common;

namespace AST.Trees
{
    internal class RootNode : Node
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
