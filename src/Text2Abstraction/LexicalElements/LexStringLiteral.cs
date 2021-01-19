namespace Text2Abstraction.LexicalElements
{
    internal class LexStringLiteral : LexElement
    {
        public LexStringLiteral(string tmp, DiagnosticInfo diagnostic) : base(diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }
    }
}