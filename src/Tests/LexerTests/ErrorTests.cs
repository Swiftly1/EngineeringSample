using System;
using Common;
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
            Assert.Equal("Unclosed string at " + DiagnosticInfo.UseTemplate(0, 3, 'd'), exception.Message);
        }

        [Fact]
        public void UnclosedMultiLineString_002()
        {
            var code = $"{Environment.NewLine}\"asd\"{Environment.NewLine}{new string(' ', 3)}\"qw";
            var result = new TextTransformer(code);
            var exception = Assert.Throws<Exception>(() => result.Walk());
            Assert.Equal("Unclosed string at " +DiagnosticInfo.UseTemplate(2, 6, 'w'), exception.Message);
        }
    }
}
