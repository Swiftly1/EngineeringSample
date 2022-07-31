using Xunit;
using Common;
using System.Linq;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;
using AST.Trees.Declarations.Typed;

namespace Tests.E2E.AST.TypeChecker
{
    public class TypeCheckerTests
    {
        [Fact]
        [Benchmark]
        public void TypeChecker_Test001()
        {
            // function call args do not match function declaration
            var code =
            @"
            namespace Test

            public int Test()
            {
	            return 5;
            }

            public void Test2()
            {
	            int test = 345645 + Test(5,Test(5,6), 3);
	            int test2 = 345645 + 3;
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.False(ast.Success);
            Assert.True(ast.Messages.Any());
            var message = "Function call of 'Test' function has incorrect number of arguments. at line number '11' at position '34' at character 'T'.";
            Assert.Equal(message, ast.Messages[0].ToString());
        }

        [Fact]
        [Benchmark]
        public void TypeChecker_Test002()
        {
            // expected int instead of string in function call
            var code =
            @"
            namespace Test

            public int Test(int a, int b)
            {
	            return 5;
            }

            public void Test2()
            {
	            int test = 345645 + Test(5, ""asd"");

                int test2 = 345645 + 3;
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.False(ast.Success);
            Assert.True(ast.Messages.Any());
            var message = "Expected expression of type 'int32' instead of 'string'. at line number '11' at position '46' at character '\"'.";
            Assert.Equal(message, ast.Messages[0].ToString());
        }

        [Fact]
        [Benchmark]
        public void TypeChecker_Test003_var()
        {
            var code =
            @"
            namespace Test

            public void Test()
            {
	            var q = 1;
	            var w = 2;
	            var y = q + w;
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);
            Assert.False(ast.Messages.Any());

            var body = ast.Data.Children[0].Children[0].Children[0];
            var q = body.Children[0] as TypedVariableDeclarationStatement;
            var w = body.Children[1] as TypedVariableDeclarationStatement;
            var y = body.Children[2] as TypedVariableDeclarationStatement;

            Assert.NotNull(q);
            Assert.NotNull(w);
            Assert.NotNull(y);

            Assert.Equal("q", q.VariableName);
            Assert.Equal("w", w.VariableName);
            Assert.Equal("y", y.VariableName);

            Assert.Equal("int32", q.TypeInfo.Name);
            Assert.Equal("int32", w.TypeInfo.Name);
            Assert.Equal("int32", y.TypeInfo.Name);

            Assert.True(y.Expression.IsConstant());
            Assert.True(y.Expression is ComplexTypedExpression);
            Assert.Equal(ExpressionOperator.Addition, (y.Expression as ComplexTypedExpression).Operator);
        }

        [Fact]
        [Benchmark]
        public void TypeChecker_Test004_functionArgNotConstant()
        {
            var code =
            @"
            namespace Test

            public int Test(int a, int b)
            {
	            if (a > b)
	            {
		            return a + b;
	            }
	            else
	            {
		            return a - Qwert();
	            }
            }

            public int Qwert()
            {
	            return 10 * 10;
            }
            ";

            var lexed = new TextTransformer(code).Walk();
            var ast = new ASTBuilder(lexed).Build();

            Assert.True(ast.Success);
            Assert.False(ast.Messages.Any());

            var typedFunction = ast.Data.Children[0].Children[0] as TypedFunctionNode;
            Assert.NotNull(typedFunction);
            Assert.True(typedFunction.ScopeContext.DeclaredVariablesList.All(d => !d.IsConstant));
        }
    }
}
