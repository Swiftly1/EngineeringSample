using System.Collections.Generic;
using Common;

namespace AST.Trees
{
    public abstract class Node
    {
        protected Node(DiagnosticInfo diag)
        {
            Diagnostics = diag;
        }

        public DiagnosticInfo Diagnostics { get; }

        public List<Node> Children { get; } = new List<Node>();

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
