using System.Collections.Generic;
using System.Linq;

namespace Text2Abstraction
{
    public static class LexingFacts
    {
        public const char EscapeChar = '\\';

        public static List<char> OtherTokens => Char2LexElementMap.Select(x => x.Key).ToList();

        public static Dictionary<char, LexingElement> Char2LexElementMap = new Dictionary<char, LexingElement>
        {
            { '(', LexingElement.OpenParenthesis },
            { ')', LexingElement.ClosedParenthesis },

            { '{', LexingElement.OpenBracket },
            { '}', LexingElement.ClosedBracket },

            { '.', LexingElement.Dot },
            { ',', LexingElement.Comma },
        };

        public static List<string> Operators => String2OperatorMap.Select(x => x.Key).ToList();

        public static Dictionary<string, LexingElement> String2OperatorMap = new Dictionary<string, LexingElement>
        {
            { "==", LexingElement.EqualEqual },
            { "!=", LexingElement.BangEqual },
            { "-=", LexingElement.MinusEqual },
            { "+=", LexingElement.PlusEqual },
            { "/=", LexingElement.SlashEqual },
            { "*=", LexingElement.StarEqual },
            { ">", LexingElement.GreaterThan },
            { "<", LexingElement.LessThan },
            { "<=", LexingElement.LessOrEqual },
            { ">=", LexingElement.GreaterOrEqual },
            { "+", LexingElement.Plus },
            { "-", LexingElement.Minus },
            { "/", LexingElement.Slash },
            { "*", LexingElement.Star },
            { "!", LexingElement.Bang },
        };
    }
}
