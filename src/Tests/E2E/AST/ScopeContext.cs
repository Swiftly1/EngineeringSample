using Xunit;
using AST.Trees;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using AST.Trees.Declarations.Typed;

namespace Tests.E2E.AST
{
    public class ScopeContext_Tests
    {
        [Fact]
        [Benchmark]
        public void ScopeContext_Test001()
        {
            var code =
            @"
            namespace TestNamespace
            public void TestFunction()
            {
                int test = 345645 +23423;
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);

            var ns = ast.Data.Children[0] as NamespaceNode;
            var tfn = ast.Data.Children[0].Children[0] as TypedFunctionNode;

            Assert.Equal("TestNamespace", ns.ScopeContext.Namespace);
            Assert.Equal("TestNamespace", tfn.ScopeContext.Parent.Namespace);
        }
    }
}