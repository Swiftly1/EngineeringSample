using Common;

namespace AST.Trees
{
    public class NamespaceNode : Node
    {
        public NamespaceNode(DiagnosticInfo diag, string name) : base(diag)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Namespace: '{Name}'";
        }
    }
}
