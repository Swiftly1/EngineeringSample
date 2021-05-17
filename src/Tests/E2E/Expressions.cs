using AST.Builders;
using AST.Trees.Expressions.Untyped;
using AST.Trees.Statements;
using BenchmarkDotNet.Attributes;
using Common;
using Text2Abstraction;
using Xunit;

namespace Tests.E2E
{
    public class Expressions
    {
        [Fact]
        [Benchmark]
        public void Test001()
        {
            var code =
            @"
            namespace TestNamespace
            {
                public void TestFunction()
	            {
		            int test = 345645 +23423;
	            }
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);

            var vdn = ast.Data.Children[0].Children[0].Children[0].Children[0] as VariableDeclarationStatement;
            Assert.NotNull(vdn);
            Assert.Equal("test", vdn.VariableName);
            Assert.Equal(TypeFacts.GetInt32().Name, vdn.DeclaredType.Name);

            var expr = vdn.Expression as ComplexUntypedExpression;

            Assert.NotNull(expr);
            Assert.Equal(ExpressionOperator.Addition, expr.Operator);
            Assert.True(expr.Left is ConstantMathUntypedExpression);
            Assert.True(expr.Right is ConstantMathUntypedExpression);

            Assert.Equal(345645, (expr.Left as ConstantMathUntypedExpression).Value);
            Assert.Equal(23423, (expr.Right as ConstantMathUntypedExpression).Value);
        }
    }
}
