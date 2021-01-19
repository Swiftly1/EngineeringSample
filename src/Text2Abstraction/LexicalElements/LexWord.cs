namespace Text2Abstraction.LexicalElements
{
    internal class LexWord : LexElement
    {
        public LexWord(string tmp, DiagnosticInfo diagnostic) : base(diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }
    }
}