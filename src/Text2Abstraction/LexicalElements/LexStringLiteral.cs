using Common;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexStringLiteral : LexElement
    {
        public LexStringLiteral(string tmp, DiagnosticInfo diagnostic) : base(LexingElement.String, diagnostic)
        {
            Value = tmp;
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"String: {Value}";
        }
    }
}