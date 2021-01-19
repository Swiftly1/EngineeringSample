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
    }
}
