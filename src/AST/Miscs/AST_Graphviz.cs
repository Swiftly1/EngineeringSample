using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AST.Trees;
using Common;

namespace AST.Miscs
{
    public class AST_Graphviz
    {
        private IMessagesPrinter _printer { get; }
        private readonly StringBuilder sb = new StringBuilder();
        private int counter = 0;

        public AST_Graphviz(IMessagesPrinter printer)
        {
            _printer = printer;
        }

        public void PrintPretty(Node node)
        {
            CollectData(node);

            var template = @$"
digraph Code
{{
    {sb}
}}";
            _printer.PrintFancy(template);
        }

        private void CollectData(Node node)
        {
            var q = new Queue<Node>();
            q.Enqueue(node);
            sb.Append(Node2Text(node) + Environment.NewLine);

            while (q.Count > 0)
            {
                var current = q.Dequeue();

                if (!current.Children.Any())
                {
                    continue;
                }

                ProcessElement(node, q, current);
            }
        }

        private void ProcessElement(Node node, Queue<Node> q, Node current)
        {
            sb.Append(" -> ");

            if (current.Children.Any())
                sb.Append("{");

            foreach (var child in current.Children)
            {
                var name = Node2Text(child) + ";";
                sb.Append(name);
            }

            if (node.Children.Any())
                sb.Append($"}}{Environment.NewLine}");

            foreach (var child in current.Children)
            {
                q.Enqueue(child);
            }
        }

        private string Node2Text(Node node)
        {
            return $"{{\"{counter++}\" [label=\"{node}\"]}}";
        }
    }
}
