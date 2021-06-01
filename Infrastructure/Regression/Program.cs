using BenchmarkDotNet.Running;
using Tests.LexerTests;

namespace Regression
{
    static class Program
    {
        static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(BasicTests).Assembly).RunAll();
        }
    }
}
