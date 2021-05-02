using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AST.Trees;
using Common;

namespace AST.Miscs
{
    public class AST_Graphviz
    {
        private IMessagesPrinter Printer { get; }
        private readonly StringBuilder sb = new();
        private int counter = 0;

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

            Printer.PrintFancy(template);
        }

        private void CollectData(Node node)
        {
            var q = new Queue<Node>();
            q.Enqueue(node);

            while (q.Count > 0)
            {
                var current = q.Dequeue();

                if (!current.Children.Any())
                {
                    continue;
                }

                ProcessElement(current, q);
            }
        }

        private void ProcessElement(Node current, Queue<Node> q)
        {
            foreach (var child in current.Children)
            {
                sb
                .Append("\t")
                .Append(Node2Text(current))
                .Append(" -> ")
                .Append(Node2Text(child))
                .AppendLine();
            }

            foreach (var child in current.Children)
            {
                q.Enqueue(child);
            }
        }

        private string Node2Text(Node node)
        {
            return $"{{\"{node.Id}\" [label=\"{node}\"]}}";
        }
    }
}
