using System;
using Common.Lexing;
using Text2Abstraction;
using Text2Abstraction.LexicalElements;
using Xunit;

namespace Tests.LexerTests
{
    public class BasicTests
    {
        [Fact]
        public void Test001()
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
        public void Test002()
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
        public void Test003()
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

            var transformer = new TextTransformer(code);
            var result = transformer.Walk();

            Assert.Equal(transformer.Settings.NewLineAware ? 19 : 11, result.Count);

            Assert.True(result[0] is LexElement);
            Assert.True(result[1] is LexKeyword);
            Assert.True(result[2] is LexWord);
            Assert.True(result[3] is LexElement);
            Assert.True(result[4] is LexCharacter);
            Assert.True(result[5] is LexElement);
            Assert.True(result[6] is LexKeyword);
            Assert.True(result[7] is LexKeyword);
            Assert.True(result[8] is LexWord);
            Assert.True(result[9] is LexCharacter);
            Assert.True(result[10] is LexCharacter);
            Assert.True(result[12] is LexCharacter);
            Assert.True(result[13] is LexElement);
            Assert.True(result[14] is LexElement);
            Assert.True(result[15] is LexCharacter);
            Assert.True(result[16] is LexElement);
            Assert.True(result[17] is LexCharacter);
            Assert.True(result[18] is LexElement);

            Assert.Equal("namespace", (result[1] as LexKeyword).Value);
            Assert.Equal("Test", (result[2] as LexWord).Value);
            Assert.Equal("public", (result[6] as LexKeyword).Value);
            Assert.Equal("void", (result[7] as LexKeyword).Value);
            Assert.Equal("Test", (result[8] as LexWord).Value);

            Assert.Equal(LexingElement.OpenBracket, (result[4] as LexCharacter).Kind);
            Assert.Equal(LexingElement.OpenParenthesis, (result[9] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedParenthesis, (result[10] as LexCharacter).Kind);
            Assert.Equal(LexingElement.OpenBracket, (result[12] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedBracket, (result[15] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedBracket, (result[17] as LexCharacter).Kind);
        }

        [Fact]
        public void Test004()
        {
            var transformer = new TextTransformer($"a{Environment.NewLine}b{Environment.NewLine}c");
            var result = transformer.Walk();

            Assert.Equal(5, result.Count);

            Assert.True(result[1].Kind == LexingElement.NewLine);
            Assert.True(result[3].Kind == LexingElement.NewLine);

            Assert.True(result[0] is LexWord);
            Assert.True(result[2] is LexWord);
            Assert.True(result[4] is LexWord);

            Assert.Equal("a", (result[0] as LexWord).Value);
            Assert.Equal("b", (result[2] as LexWord).Value);
            Assert.Equal("c", (result[4] as LexWord).Value);
        }

        [Fact]
        public void Test005()
        {
            var result = new TextTransformer("\"asd\"").Walk();

            Assert.Single(result);

            Assert.True(result[0].Kind == LexingElement.String);

            Assert.True(result[0] is LexStringLiteral);

            Assert.Equal("asd", (result[0] as LexStringLiteral).Value);
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
