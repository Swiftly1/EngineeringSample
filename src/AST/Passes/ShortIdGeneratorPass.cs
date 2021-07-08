using System.Collections.Generic;
using AST.Passes.Results;
using AST.Trees;
using AST.Trees.Miscs;

namespace AST.Passes
{
    public class ShortIdGeneratorPass : IPass
    {
        private int Counter { get; set; } = 0;

        public PassesExchangePoint Exchange { get; set; }

        public const string PassName = "ShortIdGeneratorPass";

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

        public PassResult Walk(Node root, PassesExchangePoint exchange)
        {
            Exchange = exchange;
            AttachShortIdToNodes(root);
            return new EmptyPassResult(PassName);
        }
    }
}
