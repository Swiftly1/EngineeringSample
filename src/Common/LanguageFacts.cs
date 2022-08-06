using Common.Lexing;
using System.Collections.Generic;

namespace Common
{
    public static class LanguageFacts
    {
        public const string Namespace = "namespace";

        public const string Public = "public";
        public const string Private = "private";
        public const string Internal = "internal";

        public const string Int = "int";
        public const string String = "string";
        public const string Char = "char";
        public const string Double = "double";
        public const string Void = "void";
        public const string Var = "var";

        public const string If = "if";
        public const string Else = "else";

        public const string Return = "return";
        public const string Container = "container";
        public const string New = "new";

        public static readonly Dictionary<string, LexingElement> KeywordMapper = new()
        {
            { Namespace, LexingElement.Namespace },

            { Public, LexingElement.AccessibilityModifier },
            { Private, LexingElement.AccessibilityModifier },
            { Internal, LexingElement.AccessibilityModifier },

            { Int, LexingElement.Type },
            { String, LexingElement.Type },
            { Char, LexingElement.Type },
            { Double, LexingElement.Type },
            { Void, LexingElement.Type },
            { Var, LexingElement.Type },

            { If, LexingElement.If },
            { Else, LexingElement.Else },

            { Return, LexingElement.Return },
            { Container, LexingElement.Container },
            { New, LexingElement.New },
        };
    }
}
