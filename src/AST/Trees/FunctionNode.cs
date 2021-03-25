using System;
using Common;

namespace AST.Trees
{
    public class FunctionNode : ScopeableNode
    {
        public FunctionNode(DiagnosticInfo diag, string name) : base(diag)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return $"Function {Name}";
        }
    }
}
