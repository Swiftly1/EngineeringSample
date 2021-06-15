using Common;

namespace AST.Trees.Declarations.Untyped
{
    public class UntypedFunctionNode : ScopeableNode
    {
        public UntypedFunctionNode(DiagnosticInfo diag, string name, Node body) : base(diag)
        {
            Name = name;
            Body = body;
            Children.Add(Body);
        }

        public string Name { get; }

        public Node Body { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
