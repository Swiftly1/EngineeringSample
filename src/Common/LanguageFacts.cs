using Common.Lexing;
using System.Collections.Generic;

namespace Common
{
    public static class LanguageFacts
    {
        public static readonly Dictionary<string, LexingElement> KeywordMapper = new()
        {
            { "namespace", LexingElement.Namespace },

            { "public", LexingElement.AccessibilityModifier },
            { "private", LexingElement.AccessibilityModifier },
            { "internal", LexingElement.AccessibilityModifier },

            { "int", LexingElement.Type },
            { "string", LexingElement.Type },
            { "char", LexingElement.Type },
            { "double", LexingElement.Type },
            { "void", LexingElement.Type },
            { "var", LexingElement.Type },

            { "if", LexingElement.If },
            { "else", LexingElement.Else },

            { "return", LexingElement.Return },
            { "container", LexingElement.Container },
        };
    }
}
