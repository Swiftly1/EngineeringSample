using System;
using Common;

namespace AST.Trees
{
    public class FunctionNode : ScopeableNode
    {
        public FunctionNode(DiagnosticInfo diag, string name, Node body) : base(diag)
        {
            Name = name;
            Body = body;
        }

        public string Name { get; }

        public Node Body { get; set; }

        public override string ToString()
        {
            return $"Function {Name}";
        }
    }
}
