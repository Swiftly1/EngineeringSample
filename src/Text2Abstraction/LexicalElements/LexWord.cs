using Common;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexWord : LexElement
    {
        public LexWord(string tmp, DiagnosticInfo diagnostic) : base(LexingElement.Word, diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"Word: {Value}";
        }
    }
}