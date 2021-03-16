using System.Collections.Generic;

namespace AST.Trees
{
    public abstract class Node
    {
        public List<Node> Children { get; set; } = new List<Node>();

        public Node AddChild(Node n)
        {
            this.Children.Add(n);

            return this;
        }

        public Node AddChildren(IEnumerable<Node> n)
        {
            this.Children.AddRange(n);

            return this;
        }

        public abstract override string ToString();
    }
}
