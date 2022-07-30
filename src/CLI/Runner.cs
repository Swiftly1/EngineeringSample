using System;
using AST.Miscs;
using System.IO;
using CommandLine;
using AST.Builders;
using Emitter.LLVM;
using Text2Abstraction;
using CLI.ProcessHandling;
using System.Collections.Generic;

namespace CLI;

public class Runner
{
    private readonly ConsoleMessagesPrinter printer = new();

    private readonly RunnerSettings _settings;

    public Runner(RunnerSettings settings)
    {
        _settings = settings;
    }

    public void Main(string[] args)
    {
        Parser
        .Default
        .ParseArguments<CommandLineArguments>(args)
        .WithParsed(NormalizeArgsAndRun);
    }

    private void NormalizeArgsAndRun(CommandLineArguments args)
    {
        printer.PrintColorNewLine(args.GetDisplaySettings() + Environment.NewLine);

        if (args.LLVMDirectory is null)
        {
            args.LLVMDirectory = Environment.GetEnvironmentVariable(EnvironmentVariables.LLVM_Directory);

            if (args.LLVMDirectory is null)
            {
                Console.WriteLine("Either provide path to llvm via -llvm arg or" +
                    $" set environment variable '{EnvironmentVariables.LLVM_Directory}'.");
                return;
            }
        }

        Run(args);
    }

    private void Run(CommandLineArguments args)
    {
        var code = File.ReadAllText("code.xd");
        var lexems = new TextTransformer(code, new Settings { NewLineAware = false }).Walk();

        if (args.Tokens)
        {
            foreach (var item in lexems)
            {
                printer.PrintInformationNewLine(item.ToString());
            }

            printer.PrintInformationNewLine("");
        }

        var ast = new ASTBuilder(lexems).Build();

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

            printer.Color = ConsoleColor.Yellow;
            var emit_result = new LLVM_Emitter(args.IR ? printer : null).Emit(ast.Data!);

            if (!emit_result.Success)
            {
                printer.Color = ConsoleColor.Red;
                printer.PrintColorNewLine(emit_result.Message!);
                return;
            }

            if (!new DirectoryInfo(args.LLVMDirectory).Exists)
            {
                printer.Color = ConsoleColor.Red;
                printer.PrintColorNewLine($"LLVM directory not found. Path: '{args.LLVMDirectory}'"!);
                return;
            }

            SaveIRAndRunLLVM(args, emit_result.Data);
        }
        else
        {
            foreach (var error in ast.Messages)
            {
                printer.PrintErrorNewLine(error.ToString());
            }
        }
    }

    private void SaveIRAndRunLLVM(CommandLineArguments args, string ir)
    {
        printer.Color = ConsoleColor.White;

        var ir_path = Path.Combine(args.LLVMDirectory, "main.ll");
        var o_path = Path.Combine(args.LLVMDirectory, "main.o");
        var optimized_path = Path.Combine(args.LLVMDirectory, "main_optimized.ll");
        var wasm_path = Path.Combine(args.LLVMDirectory, "main.wasm");

        File.WriteAllText(ir_path, ir);

        File.Delete(o_path);
        File.Delete(optimized_path);
        File.Delete(wasm_path);

        var opt_path = Path.Combine(args.LLVMDirectory, _settings.LLVM_opt);
        var llc_path = Path.Combine(args.LLVMDirectory, _settings.LLVM_llc);
        var wasm_ld = Path.Combine(args.LLVMDirectory, _settings.LLVM_wasmld);

        var programs = new List<ExecuteProgramInfo>
        {
            new ExecuteProgramInfo
            {
                Path = opt_path,
                Args = "main.ll -O2 -S -strip -o main_optimized.ll",
                ErrorMessage = "Something went wrong when trying to optimize IR, here are details:",
                SuccessMessage = "Successfully optimized"
            },
            new ExecuteProgramInfo
            {
                Path = llc_path,
                Args = "-mtriple=wasm32-unknown-unknown -O3 -filetype=obj main_optimized.ll -o main.o",
                ErrorMessage = "Something went wrong when trying to generate native object files, here are details:",
                SuccessMessage = "Successfully generated native object files"
            },
            new ExecuteProgramInfo
            {
                Path = wasm_ld,
                Args = "main.o -o main.wasm --no-entry -allow-undefined --export-all",
                ErrorMessage = "Something went wrong when trying to generate WebAssembly, here are details:",
                SuccessMessage = "Successfully generated WebAssembly files"
            },
        };

        foreach (var prog in programs)
        {
            var executionResult = ProcessHelper.ExecuteProgram(prog.Path, prog.Args);

            if (executionResult.Success)
            {
                printer.Color = ConsoleColor.Green;
                printer.PrintColorNewLine(prog.SuccessMessage);
            }
            else
            {
                printer.Color = ConsoleColor.Red;
                printer.PrintColorNewLine(prog.ErrorMessage);
                printer.PrintColorNewLine(prog.Path);
                printer.PrintColorNewLine("Error log: ");
                printer.PrintColorNewLine(executionResult.ErrorLog);
                printer.PrintColorNewLine("Output log: ");
                printer.PrintColorNewLine(executionResult.OutputLog);
                return;
            }
        }
    }
}