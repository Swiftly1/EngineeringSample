using Common;
using System;

namespace AST.Trees.Declarations.Typed
{
    public class TypedFunctionNode : ScopeableNode
    {
        public TypedFunctionNode(DiagnosticInfo diag, string name, Node body, TypeInfo type) : base(diag, null)
        {
            // TODO: Handle Scope
            throw new NotImplementedException("handle scope node");
            Name = name;
            Type = type;
            Children.Add(body);
        }

        public string Name { get; }

        public Node Body => Children[0];

        public TypeInfo Type { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
