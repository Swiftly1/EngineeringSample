using Xunit;
using AST.Trees;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using AST.Trees.Statements.Typed;
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

            var nameSpaceNode = ast.Data.Children[0] as NamespaceNode;
            var typedFuncNode = ast.Data.Children[0].Children[0] as TypedFunctionNode;

            Assert.Equal("TestNamespace", nameSpaceNode.ScopeContext.Name);
            Assert.Equal("TestNamespace", typedFuncNode.ScopeContext.Parent.Name);
        }

        [Fact]
        [Benchmark]
        public void ScopeContext_Test002()
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

            var vdn = ast.Data.Children[0].Children[0].Children[0].Children[0] as TypedVariableDeclarationStatement;

            Assert.Equal("function_TestFunction", vdn.ScopeContext.Name);
            Assert.Equal("TestNamespace", vdn.ScopeContext.Parent.Name);
        }
    }
}