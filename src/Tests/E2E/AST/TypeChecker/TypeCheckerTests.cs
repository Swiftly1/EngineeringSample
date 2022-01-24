using Xunit;
using System.Linq;
using AST.Builders;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;

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
    }
}
