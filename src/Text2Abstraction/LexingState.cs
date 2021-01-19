namespace Text2Abstraction
{
    internal enum LexingState
    {
        Root,
        String,
        Word,
        NumericalValue,
        WhiteCharacter,
        OtherSyntax,
        Unknown
    }
}
