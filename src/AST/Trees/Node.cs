using System;
using Common;
using System.Collections.Generic;

namespace AST.Trees
{
    public abstract class Node
    {
        protected Node(DiagnosticInfo diag)
        {
            Diagnostics = diag;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();

        public DiagnosticInfo Diagnostics { get; private set; }

        public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public List<Node> Children { get; private set; } = new List<Node>();

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

        public Node CopyTo(Node other)
        {
            other.Properties.Clear();

            foreach (var prop in Properties)
                other.Properties.Add(prop.Key, prop.Value);

            other.Id = this.Id;
            other.Diagnostics = this.Diagnostics;

            return other;
        }
    }
}
