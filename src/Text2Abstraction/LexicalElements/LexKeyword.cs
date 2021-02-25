using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexKeyword : LexElement
    {
        public LexKeyword(string tmp, LexingElement type, DiagnosticInfo diagnostic) : base(type, diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"Keyword: {Value}";
        }
    }
}