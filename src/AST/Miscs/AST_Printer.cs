﻿using AST.Trees;
using Common;

namespace AST.Miscs
{
    public class AST_Printer
    {
        public IMessagesPrinter Printer { get; }

        public AST_Printer(IMessagesPrinter printer)
        {
            Printer = printer;
        }

        public void PrintPretty(Node node)
        {
            PrintPretty(node, "", false);
        }

        private void PrintPretty(Node node, string indent, bool last)
        {
            Printer.PrintFancy(indent);
            if (last)
            {
                Printer.PrintFancy(@"\-");
                indent += "  ";
            }
            else
            {
                Printer.PrintFancy("|-");
                indent += "| ";
            }

            Printer.PrintFancyNewLine($"{node}");

            var children = node.Children;
            for (int i = 0; i < children.Count; i++)
                PrintPretty(children[i], indent, i == children.Count - 1);
        }
    }
}
