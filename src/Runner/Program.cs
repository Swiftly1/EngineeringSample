using System;
using System.IO;
using AST.Builders;
using AST.Miscs;
using Text2Abstraction;

namespace Runner
{
    internal static class Program
    {
        private static void Main()
        {
            var code = File.ReadAllText("code.xd");
            var test = new TextTransformer(code, new Settings { NewLineAware = false}).Walk();

            foreach (var item in test)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            var ast = new ASTBuilder(test).Build();
            var printer = new ConsoleMessagesPrinter();

            if (ast.Success)
            {
                new AST_Graphviz(printer).PrintPretty(ast.Data);
                AST_Printer.PrintPretty(ast.Data, printer);
            }
            else
            {
                foreach (var error in ast.Messages)
                {
                    printer.PrintError(error.ToString());
                }
            }
        }
    }
}
