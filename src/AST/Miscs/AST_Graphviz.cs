using System;
using Common;
using AST.Trees;
using System.Text;
using AST.Trees.Miscs;
using System.Collections.Generic;

namespace AST.Miscs
{
    public class AST_Graphviz
    {
        private IMessagesPrinter Printer { get; }

        private readonly StringBuilder sb = new();

        private HashSet<int> AlreadyIncluded = new();

        public AST_Graphviz(IMessagesPrinter printer)
        {
            Printer = printer;
        }

        public void PrintPretty(Node node)
        {
            CollectData(node);

            var template = $"digraph Code" + Environment.NewLine +
                            "{" + Environment.NewLine +
                            $"{sb}" +
                            "}";

            Printer.PrintColor(template);
        }

        private void CollectData(Node node)
        {
            var queue = new Queue<Node>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                ProcessElement(queue.Dequeue(), queue);
            }
        }

        private void ProcessElement(Node current, Queue<Node> q)
        {
            if (current.Children.Count > 0)
            {
                foreach (var child in current.Children)
                {
                    var childId = (int)child[NodeProperties.ShortId];

                    AlreadyIncluded.Add(childId);

                    sb
                    .Append('\t')
                    .Append(Node2Text(current))
                    .Append(" -> ")
                    .AppendLine(Node2Text(child));
                }

                foreach (var child in current.Children)
                {
                    q.Enqueue(child);
                }
            }
            else
            {
                var currentId = (int)current[NodeProperties.ShortId];

                if (!AlreadyIncluded.Contains(currentId))
                {
                    AlreadyIncluded.Add(currentId);

                    sb
                    .Append('\t')
                    .AppendLine(Node2Text(current));
                }
            }
        }

        private string Node2Text(Node node)
        {
            return $"{{\"{node[NodeProperties.ShortId]}\" [label=\"{node}\"]}}";
        }
    }
}
