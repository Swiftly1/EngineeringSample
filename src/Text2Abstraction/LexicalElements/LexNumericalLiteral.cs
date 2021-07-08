using Common;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexNumericalLiteral : LexElement
    {
        public LexNumericalLiteral(string tmp, DiagnosticInfo diagnostic) : base(LexingElement.Numerical, diagnostic)
        {
            StringValue = tmp;
        }

        public string StringValue { get; set; }

        public override string ToString()
        {
            return $"Numerical: {StringValue}";
        }
    }
}