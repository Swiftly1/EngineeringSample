using System.Collections.Generic;
using System.Linq;
using Common.Lexing;

namespace Text2Abstraction
{
    public static class LexingFacts
    {
        public const char EscapeChar = '\\';

        public static List<char> OtherTokens
        {
            get
            {
                var p1 = LexElementsKeys.SelectMany(x => x).ToList();
                var p2 = OperatorsKeys.SelectMany(x => x).ToList();
                p1.AddRange(p2);

                return p1;
            }
        }

        public static List<string> CombinedMapKeys => CombinedMap.Select(x => x.Key).ToList();

        public static Dictionary<string, LexingElement> CombinedMap
        {
            get
            {
                var p1 = String2LexElementMap.ToList();
                var p2 = String2OperatorMap.ToList();
                p1.AddRange(p2);

                return p1.ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public static List<string> LexElementsKeys => String2LexElementMap.Select(x => x.Key).ToList();

        public static Dictionary<string, LexingElement> String2LexElementMap = new Dictionary<string, LexingElement>
        {
            { "(", LexingElement.OpenParenthesis },
            { ")", LexingElement.ClosedParenthesis },
            { "{", LexingElement.OpenBracket },
            { "}", LexingElement.ClosedBracket },
            { ".", LexingElement.Dot },
            { ";", LexingElement.SemiColon },
            { ",", LexingElement.Comma },
            { "=>", LexingElement.Lambda },
        };

        public static List<string> OperatorsKeys => String2OperatorMap.Select(x => x.Key).ToList();

        public static Dictionary<string, LexingElement> String2OperatorMap = new Dictionary<string, LexingElement>
        {
            { "=", LexingElement.Equal },
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
