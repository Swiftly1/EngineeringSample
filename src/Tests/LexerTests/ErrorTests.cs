using System;
using Text2Abstraction;
using Xunit;

namespace Tests.LexerTests
{
    public class ErrorTests
    {
        [Fact]
        public void UnclosedString_001()
        {
            var result = new TextTransformer("\"asd");

            var exception = Assert.Throws<Exception>(() => result.Walk());
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Unclosed string at line number '0' at position '3' around character 'd'.", exception.Message);
        }

        [Fact]
        public void UnclosedMultiLineString_002()
        {
            var code = $"{Environment.NewLine}\"asd\"{Environment.NewLine}{new string(' ', 3)}\"qw";
            var result = new TextTransformer(code);

            var exception = Assert.Throws<Exception>(() => result.Walk());
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal("Unclosed string at line number '2' at position '6' around character 'w'.", exception.Message);
        }
    }
}
