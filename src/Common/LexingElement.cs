namespace Common.Lexing
{
    public enum LexingElement
    {
        OpenBracket,
        ClosedBracket,
        Comma,
        Dot,
        OpenParenthesis,
        ClosedParenthesis,
        NewLine,

        Numerical,
        String,
        Word,
        Equal,
        EqualEqual,
        BangEqual,
        MinusEqual,
        PlusEqual,
        SlashEqual,
        StarEqual,
        GreaterThan,
        LessThan,
        LessOrEqual,
        GreaterOrEqual,
        Plus,
        Minus,
        Slash,
        Star,
        Bang,

        AccessibilityModifier,
        Namespace,
        Type,
        Lambda,
        SemiColon,
    }
}
