using Xunit;
using Common;
using AST.Builders;
using Text2Abstraction;
using AST.Trees.Statements;
using BenchmarkDotNet.Attributes;
using AST.Trees.Expressions.Typed;
using AST.Trees.Expressions.Untyped;

namespace Tests.E2E.AST
{
    public class Expressions
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

            var vdn = ast.Data.Children[0].Children[0].Children[0].Children[0] as VariableDeclarationStatement;
            Assert.NotNull(vdn);
            Assert.Equal("test", vdn.VariableName);
            Assert.Equal("int", vdn.DesiredType);

            var expr = vdn.Expression as ComplexTypedExpression;

            Assert.NotNull(expr);
            Assert.Equal(ExpressionOperator.Addition, expr.Operator);
            Assert.True(expr.Left is ConstantMathTypedExpression);
            Assert.True(expr.Right is ConstantMathTypedExpression);

            Assert.Equal(345645, (expr.Left as ConstantMathTypedExpression).Value);
            Assert.Equal(23423, (expr.Right as ConstantMathTypedExpression).Value);
        }
    }
}
