using System;
using System.IO;
using Text2Abstraction;

namespace Runner
{
    static class Program
    {
        static void Main()
        {
            var code = File.ReadAllText("code.xd");
            var test = new TextTransformer(code).Walk();

            foreach (var item in test)
            {
                Console.WriteLine(item);
            }
        }
    }
}
