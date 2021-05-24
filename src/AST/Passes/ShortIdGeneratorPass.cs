using System.Collections.Generic;
using AST.Trees;
using AST.Trees.Miscs;

namespace AST.Passes
{
    public class ShortIdGeneratorPass : IPass
    {
        private int Counter { get; set; } = 0;

        private void AttachShortIdToNodes(Node root)
        {
            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                current[NodeProperties.ShortId] = Counter++;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public void Walk(Node root)
        {
            AttachShortIdToNodes(root);
        }
    }
}
