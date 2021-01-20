using System;
using Text2Abstraction;
using Text2Abstraction.LexicalElements;
using Xunit;

namespace Tests
{
    public class BasicTests
    {
        [Fact]
        public void Test1()
        {
            var result = new TextTransformer("abc test a123").Walk();

            Assert.Equal(3, result.Count);

            Assert.True(result[0] is LexWord);
            Assert.True(result[1] is LexWord);
            Assert.True(result[2] is LexWord);

            Assert.Equal("abc", (result[0] as LexWord).Value);
            Assert.Equal("test", (result[1] as LexWord).Value);
            Assert.Equal("a123", (result[2] as LexWord).Value);
        }

        [Fact]
        public void Test2()
        {
            var result = new TextTransformer("12 35.5 7.7 6").Walk();

            Assert.Equal(4, result.Count);

            Assert.True(result[0] is LexNumericalLiteral);
            Assert.True(result[1] is LexNumericalLiteral);
            Assert.True(result[2] is LexNumericalLiteral);
            Assert.True(result[3] is LexNumericalLiteral);

            Assert.Equal(12, (result[0] as LexNumericalLiteral).IntegerValue);
            Assert.Equal(35.5, (result[1] as LexNumericalLiteral).DoubleValue);
            Assert.Equal(7.7, (result[2] as LexNumericalLiteral).DoubleValue);
            Assert.Equal(6, (result[3] as LexNumericalLiteral).IntegerValue);
        }

        [Fact]
        public void Test3()
        {
            var code =
            @"
            namespace Test
            {
	            public void Test()
	            {

	            }
            }
            ";

            var result = new TextTransformer(code).Walk();

            Assert.Equal(11, result.Count);

            Assert.True(result[0] is LexWord);
            Assert.True(result[1] is LexWord);
            Assert.True(result[2] is LexCharacter);
            Assert.True(result[3] is LexWord);
            Assert.True(result[4] is LexWord);
            Assert.True(result[5] is LexWord);
            Assert.True(result[6] is LexCharacter);
            Assert.True(result[7] is LexCharacter);
            Assert.True(result[8] is LexCharacter);
            Assert.True(result[9] is LexCharacter);
            Assert.True(result[10] is LexCharacter);

            Assert.Equal("namespace", (result[0] as LexWord).Value);
            Assert.Equal("Test", (result[1] as LexWord).Value);
            Assert.Equal("public", (result[3] as LexWord).Value);
            Assert.Equal("void", (result[4] as LexWord).Value);
            Assert.Equal("Test", (result[5] as LexWord).Value);

            Assert.Equal(LexingElement.OpenBracket, (result[2] as LexCharacter).Kind);
            Assert.Equal(LexingElement.OpenParenthesis, (result[6] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedParenthesis, (result[7] as LexCharacter).Kind);
            Assert.Equal(LexingElement.OpenBracket, (result[8] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedBracket, (result[9] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedBracket, (result[10] as LexCharacter).Kind);
        }

        [Fact]
        public void ChineseTextTest()
        {
            var result = new TextTransformer("早上好 再見 你好").Walk();

            Assert.Equal(3, result.Count);

            Assert.True(result[0] is LexWord);
            Assert.True(result[1] is LexWord);
            Assert.True(result[2] is LexWord);

            Assert.Equal("早上好", (result[0] as LexWord).Value);
            Assert.Equal("再見", (result[1] as LexWord).Value);
            Assert.Equal("你好", (result[2] as LexWord).Value);
        }
    }
}
