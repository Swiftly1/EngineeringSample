using System.Collections.Generic;

namespace AST.Trees.Miscs
{
    public class NodeShortIdGenerator
    {
        private Node Node { get; }

        private static int Counter { get; set; } = 0;

        public NodeShortIdGenerator(Node node)
        {
            Node = node;
        }

        public Node AttachShortIdToNodes()
        {
            var queue = new Queue<Node>();
            queue.Enqueue(Node);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                current[NodeProperties.ShortId] = Counter++;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }

            return Node;
        }
    }
}
