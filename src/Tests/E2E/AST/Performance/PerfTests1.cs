using Xunit;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using System.Text;

namespace Tests.E2E.AST.Performance
{
    public class PerfTests1
    {
        [Fact]
        [Benchmark]
        public void Performance_Many_SimpleFunctions_Test001()
        {
            var code =
            @"
            namespace TestNamespace{0}
            public void TestFunction{1}()
            {{
                int test = 345645 +23423;
            }}
            ";

            var sb = new StringBuilder();

            for (int i = 0; i < 100; i++)
            {
                sb.Append(string.Format(code, i, i));
            }

            var lexed = new TextTransformer(sb.ToString(), new Settings { NewLineAware = false }).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);
            Assert.Equal(100, ast.Data.Children.Count);
        }
    }
}
