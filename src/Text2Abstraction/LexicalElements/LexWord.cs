namespace Text2Abstraction.LexicalElements
{
    public class LexWord : LexElement
    {
        public LexWord(string tmp, DiagnosticInfo diagnostic) : base(diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }
    }
}