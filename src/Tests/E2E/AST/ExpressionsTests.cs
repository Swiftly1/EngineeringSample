using Xunit;
using Common;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;
using AST.Trees.Expressions.Untyped;

namespace Tests.E2E.AST
{
    public class ExpressionsTests
    {
        [Fact]
        [Benchmark]
        public void Expressions_Test001()
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
            Assert.NotNull(vdn);
            Assert.Equal("test", vdn.VariableName);
            Assert.Equal("int", vdn.TypeInfo.Name);

            var expr = vdn.Expression as ComplexTypedExpression;

            Assert.NotNull(expr);
            Assert.Equal(ExpressionOperator.Addition, expr.Operator);
            Assert.True(expr.Left is ConstantTypedExpression);
            Assert.True(expr.Right is ConstantTypedExpression);

            Assert.Equal(345645, (expr.Left as ConstantTypedExpression).Value);
            Assert.Equal(23423, (expr.Right as ConstantTypedExpression).Value);
        }
    }
}
