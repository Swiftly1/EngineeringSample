using System;
using System.IO;
using AST.Builders;
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

            var ast = new ASTBuilder(test).Build();
        }
    }
}
