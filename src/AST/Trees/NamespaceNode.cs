using Common;

namespace AST.Trees
{
    public class NamespaceNode : ScopeableNode
    {
        public NamespaceNode(DiagnosticInfo diag, string name) : base(diag, new ScopeContext())
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
