namespace AST.Trees
{
    public class NamespaceNode : Node
    {
        public NamespaceNode(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"Namespace: {Name}";
        }
    }
}
