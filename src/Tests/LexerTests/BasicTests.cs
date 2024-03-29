﻿using System;
using BenchmarkDotNet.Attributes;
using Common.Lexing;
using Text2Abstraction;
using Text2Abstraction.LexicalElements;
using Xunit;

namespace Tests.LexerTests
{
    public class BasicTests
    {
        [Fact]
        [Benchmark]
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
        [Benchmark]
        public void Test002()
        {
            var result = new TextTransformer("12 35.5 7.7 6").Walk();

            Assert.Equal(4, result.Count);

            Assert.True(result[0] is LexNumericalLiteral);
            Assert.True(result[1] is LexNumericalLiteral);
            Assert.True(result[2] is LexNumericalLiteral);
            Assert.True(result[3] is LexNumericalLiteral);

            Assert.Equal("12", (result[0] as LexNumericalLiteral).StringValue);
            Assert.Equal("35.5", (result[1] as LexNumericalLiteral).StringValue);
            Assert.Equal("7.7", (result[2] as LexNumericalLiteral).StringValue);
            Assert.Equal("6", (result[3] as LexNumericalLiteral).StringValue);
        }

        [Fact]
        [Benchmark]
        public void Test003()
        {
            var code =
            @"
            namespace Test
	        public void Test()
	        {

	        }
            ";

            var transformer = new TextTransformer(code);
            var result = transformer.Walk();

            Assert.Equal(transformer.Settings.NewLineAware ? 15 : 9, result.Count);

            Assert.True(result[0] is LexElement);
            Assert.True(result[1] is LexKeyword);
            Assert.True(result[2] is LexWord);
            Assert.True(result[3] is LexElement);
            Assert.True(result[4] is LexKeyword);
            Assert.True(result[5] is LexKeyword);
            Assert.True(result[6] is LexWord);
            Assert.True(result[7] is LexCharacter);
            Assert.True(result[8] is LexCharacter);
            Assert.True(result[9] is LexElement);
            Assert.True(result[10] is LexCharacter);
            Assert.True(result[12] is LexElement);
            Assert.True(result[12] is LexElement);
            Assert.True(result[13] is LexCharacter);
            Assert.True(result[14] is LexElement);

            Assert.Equal("namespace", (result[1] as LexKeyword).Value);
            Assert.Equal("Test", (result[2] as LexWord).Value);
            Assert.Equal("public", (result[4] as LexKeyword).Value);
            Assert.Equal("void", (result[5] as LexKeyword).Value);
            Assert.Equal("Test", (result[6] as LexWord).Value);

            Assert.Equal(LexingElement.OpenParenthesis, (result[7] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedParenthesis, (result[8] as LexCharacter).Kind);
            Assert.Equal(LexingElement.OpenBracket, (result[10] as LexCharacter).Kind);
            Assert.Equal(LexingElement.ClosedBracket, (result[13] as LexCharacter).Kind);
        }

        [Fact]
        [Benchmark]
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
        [Benchmark]
        public void Test005()
        {
            var result = new TextTransformer("\"asd\"").Walk();

            Assert.Single(result);

            Assert.True(result[0].Kind == LexingElement.String);

            Assert.True(result[0] is LexStringLiteral);

            Assert.Equal("asd", (result[0] as LexStringLiteral).Value);
        }

        [Fact]
        [Benchmark]
        public void Test006()
        {
            var result = new TextTransformer("5==6").Walk();

            Assert.True(result[0].Kind == LexingElement.Numerical);
            Assert.True(result[0] is LexNumericalLiteral);
            Assert.Equal("5", (result[0] as LexNumericalLiteral).StringValue);

            Assert.True(result[1].Kind == LexingElement.EqualEqual);
            Assert.True(result[1] is LexCharacter);

            Assert.True(result[2].Kind == LexingElement.Numerical);
            Assert.True(result[2] is LexNumericalLiteral);
            Assert.Equal("6", (result[2] as LexNumericalLiteral).StringValue);
        }

        [Fact]
        [Benchmark]
        public void Test007()
        {
            var result = new TextTransformer("A=>(1+2)").Walk();

            Assert.True(result[0].Kind == LexingElement.Word);
            Assert.True(result[0] is LexWord);
            Assert.Equal("A", (result[0] as LexWord).Value);

            Assert.True(result[1].Kind == LexingElement.Lambda);
            Assert.True(result[1] is LexCharacter);

            Assert.True(result[2].Kind == LexingElement.OpenParenthesis);
            Assert.True(result[2] is LexCharacter);

            Assert.True(result[3].Kind == LexingElement.Numerical);
            Assert.True(result[3] is LexNumericalLiteral);
            Assert.Equal("1", (result[3] as LexNumericalLiteral).StringValue);

            Assert.True(result[4].Kind == LexingElement.Plus);
            Assert.True(result[4] is LexCharacter);

            Assert.True(result[5].Kind == LexingElement.Numerical);
            Assert.True(result[5] is LexNumericalLiteral);
            Assert.Equal("2", (result[5] as LexNumericalLiteral).StringValue);

            Assert.True(result[6].Kind == LexingElement.ClosedParenthesis);
            Assert.True(result[6] is LexCharacter);
        }

        [Fact]
        [Benchmark]
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
