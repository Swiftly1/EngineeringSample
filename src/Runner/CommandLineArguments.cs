#nullable enable
using CommandLine;

namespace Runner
{
    internal class CommandLineArguments
    {
        [Option("dir", Default = null, Required = false, HelpText = "Path to the project file(s)")]
        public string? WorkingDirectory { get; set; }

        [Option("llvm", Default = null, Required = false, HelpText = "Path to folder with LLVM binaries")]
        public string? LLVMDirectory { get; set; }

        [Option("ir", HelpText = "Use '--ir' to display intermediate representation.")]
        public bool IR { get; set; }

        [Option("tree", HelpText = "Use '--tree' to display abstracy syntax tree.")]
        public bool Tree { get; set; }

        [Option("tokens", HelpText = "Use '--tokens' to display tokens detected.")]
        public bool Tokens { get; set; }

        [Option("graph", HelpText = "Use '--graph' to display graphviz.")]
        public bool Graph { get; set; }

        public string GetDisplaySettings()
        {
            return $"IR: {IR}; Tree: {Tree}; Tokens: {Tokens}; Graph: {Graph}";
        }
    }
}
