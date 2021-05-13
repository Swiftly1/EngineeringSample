using System;
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

        public Guid Id { get; set; } = Guid.NewGuid();

        public DiagnosticInfo Diagnostics { get; }

        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

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

        public object this[string key]
        {
            get { return Properties[key]; }
            set { Properties[key] = value; }
        }
    }
}
