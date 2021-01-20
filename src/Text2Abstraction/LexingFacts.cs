using System.Collections.Generic;

namespace Text2Abstraction
{
    public static class LexingFacts
    {
        public const char EscapeChar = '\\';

        public static List<char> OtherTokens = new List<char> { '.', ',', '(', ')', '{', '}' };

        public static Dictionary<char, LexingElement> Char2LexElementMap = new Dictionary<char, LexingElement>
        {
            { '(', LexingElement.OpenParenthesis },
            { ')', LexingElement.ClosedParenthesis },

            { '{', LexingElement.OpenBracket },
            { '}', LexingElement.ClosedBracket },

            { '.', LexingElement.Dot },
            { ',', LexingElement.Comma },

            { '+', LexingElement.PlusSign },
            { '-', LexingElement.MinusSign },
            { '*', LexingElement.MultiplicationSign },
            { '/', LexingElement.DivisionSign },
        };
    }
}
