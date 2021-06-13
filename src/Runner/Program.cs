using System;
using System.IO;
using AST.Builders;
using AST.Miscs;
using Emitter.LLVM;
using Text2Abstraction;

namespace Runner
{
    internal static class Program
    {
        private static void Main()
        {
            var printer = new ConsoleMessagesPrinter();
            var code = File.ReadAllText("code.xd");
            var test = new TextTransformer(code, new Settings { NewLineAware = false}).Walk();

            foreach (var item in test)
            {
                printer.PrintInformationNewLine(item.ToString());
            }

            printer.PrintInformationNewLine("");

            var ast = new ASTBuilder(test).Build();

            if (ast.Success)
            {
                printer.Color = ConsoleColor.Blue;
                new AST_Printer(printer).PrintPretty(ast.Data);

                printer.PrintColorNewLine("");
                printer.Color = ConsoleColor.Magenta;
                new AST_Graphviz(printer).PrintPretty(ast.Data);

                printer.PrintColorNewLine($"{Environment.NewLine}");
                printer.Color = ConsoleColor.Green;
                new LLVM_Emitter(printer).Emit(ast.Data);
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
