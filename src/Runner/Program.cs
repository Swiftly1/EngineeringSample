using System;
using System.IO;
using AST.Miscs;
using Emitter.LLVM;
using AST.Builders;
using Text2Abstraction;
using CommandLine;
using System.Collections.Generic;

namespace Runner
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser
            .Default
            .ParseArguments<CommandLineArguments>(args)
            .WithParsed(NormalizeArgs)
            .WithNotParsed(HandleParseError);
        }

        private static void NormalizeArgs(CommandLineArguments args)
        {
            Console.WriteLine(args.GetDisplaySettings() + Environment.NewLine);

            if (args.LLVMDirectory is null)
            {
                var envDir = Environment.GetEnvironmentVariable(EnvironmentVariables.LLVM_Directory);
                args.LLVMDirectory = envDir;

                if (args.LLVMDirectory is null)
                {
                    Console.WriteLine($"Either provide path to llvm via -llvm arg or" +
                        $" set environment variable {EnvironmentVariables.LLVM_Directory}");
                    return;
                }
            }

            InternalMain(args);
        }

        private static void InternalMain(CommandLineArguments args)
        {
            var printer = new ConsoleMessagesPrinter();
            var code = File.ReadAllText("code.xd");
            var test = new TextTransformer(code, new Settings { NewLineAware = false }).Walk();

            if (args.Tokens)
            {
                foreach (var item in test)
                {
                    printer.PrintInformationNewLine(item.ToString());
                }

                printer.PrintInformationNewLine("");
            }

            var ast = new ASTBuilder(test).Build();

            if (ast.Success)
            {
                if (args.Tree)
                {
                    printer.Color = ConsoleColor.Blue;
                    new AST_Printer(printer).PrintPretty(ast.Data);
                    printer.PrintColorNewLine("");
                }

                if (args.Graph)
                {
                    printer.Color = ConsoleColor.Magenta;
                    new AST_Graphviz(printer).PrintPretty(ast.Data);
                    printer.PrintColorNewLine($"{Environment.NewLine}");
                }

                if (args.IR)
                {
                    printer.Color = ConsoleColor.Green;
                    var emit_result = new LLVM_Emitter(printer).Emit(ast.Data);

                    if (!emit_result.Success)
                    {
                        printer.Color = ConsoleColor.Red;
                        printer.PrintColorNewLine(emit_result.Message);
                    }
                }
            }
            else
            {
                foreach (var error in ast.Messages)
                {
                    printer.PrintErrorNewLine(error.ToString());
                }
            }
        }
        private static void HandleParseError(IEnumerable<Error> errs)
        {
            Console.WriteLine("Please provide/fix arguments");

            foreach (var err in errs)
            {
                Console.WriteLine($"\t{err.Tag}");
            }
        }
    }
}
