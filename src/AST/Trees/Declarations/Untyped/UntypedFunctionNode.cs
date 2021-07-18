using Common;
using AST.Trees.Miscs;
using System.Collections.Generic;

namespace AST.Trees.Declarations.Untyped
{
    public class UntypedFunctionNode : ScopeableNode
    {
        public UntypedFunctionNode(DiagnosticInfo diag, string name, string desiredType, BodyNode body, List<Argument> args) : base(diag, new ScopeContext())
        {
            Name = name;
            DesiredType = desiredType;
            Body = body;
            Arguments = args;
            Children.Add(Body);
        }

        public string Name { get; }

        public string DesiredType { get; set; }

        public BodyNode Body { get; set; }

        public List<Argument> Arguments { get; set; } = new List<Argument>();

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
