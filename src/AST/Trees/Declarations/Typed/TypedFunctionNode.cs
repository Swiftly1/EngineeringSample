using Common;

namespace AST.Trees.Declarations.Typed
{
    public class TypedFunctionNode : ScopeableNode
    {
        public TypedFunctionNode(DiagnosticInfo diag, string name, Node body, TypeInfo type) : base(diag)
        {
            Name = name;
            Body = body;
            Type = type;
            Children.Add(Body);
        }

        public string Name { get; }

        public Node Body { get; set; }

        public TypeInfo Type { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
