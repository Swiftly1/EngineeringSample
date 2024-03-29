﻿using Xunit;
using Common;
using System;
using Text2Abstraction;
using BenchmarkDotNet.Attributes;

namespace Tests.LexerTests
{
    public class ErrorTests
    {
        [Fact]
        [Benchmark]
        public void UnclosedString_001()
        {
            var result = new TextTransformer("\"asd");
            var exception = Assert.Throws<Exception>(() => result.Walk());
            Assert.Equal("Unclosed string at " + DiagnosticInfo.UseTemplate(1, 3, 'd'), exception.Message);
        }

        [Fact]
        [Benchmark]
        public void UnclosedMultiLineString_002()
        {
            // The code is:
            //`
            //"asd"
            //   "qw`

            var code = $"{Environment.NewLine}\"asd\"{Environment.NewLine}{new string(' ', 3)}\"qw";
            var result = new TextTransformer(code);
            var exception = Assert.Throws<Exception>(() => result.Walk());
            Assert.Equal("Unclosed string at " + DiagnosticInfo.UseTemplate(3, 6, 'w'), exception.Message);
        }
    }
}
