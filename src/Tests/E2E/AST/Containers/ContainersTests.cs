using Xunit;
using System;
using Common;
using System.Linq;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;

namespace Tests.E2E.AST.Containers
{
    public class ContainersTests
    {
        [Fact]
        [Benchmark]
        public void Containers_Test001_fields()
        {
            var code =
            @"
            namespace Test

            public container Box
            {
	            int a,
	            int b
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);
            Assert.False(ast.Messages.Any());

            //var container = ast.Data.Children[0].Children[0] as TypedContainer;

            //Assert.NotNull(container);

            throw new NotImplementedException();
        }
    }
}