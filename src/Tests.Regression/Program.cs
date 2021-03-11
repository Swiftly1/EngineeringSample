using BenchmarkDotNet.Running;
using Tests.LexerTests;

namespace Tests.Regression
{
    public static class Program
    {
        static void Main()
        {
            BenchmarkSwitcher.FromAssembly(typeof(BasicTests).Assembly).RunAll();
        }
    }
}
