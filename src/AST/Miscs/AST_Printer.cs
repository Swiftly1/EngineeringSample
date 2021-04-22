using System;
using AST.Trees;
using Common;

namespace AST.Miscs
{
    public static class AST_Printer
    {
        public static void PrintPretty(Node node, IMessagesPrinter printer)
        {
            PrintPretty(node, "", false, printer);
        }

        private static void PrintPretty(Node node, string indent, bool last, IMessagesPrinter printer)
        {
            printer.PrintFancy(indent);
            if (last)
            {
                printer.PrintFancy(@"\-");
                indent += "  ";
            }
            else
            {
                printer.PrintFancy("|-");
                indent += "| ";
            }

            printer.PrintFancyNewLine($"{node}");

            var children = node.Children;
            for (int i = 0; i < children.Count; i++)
                PrintPretty(children[i], indent, i == children.Count - 1, printer);
        }
    }
}
