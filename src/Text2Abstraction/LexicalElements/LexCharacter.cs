using System;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexCharacter : LexElement
    {
        public LexCharacter(LexingElement kind, DiagnosticInfo diagnostic) : base(kind, diagnostic)
        {
        }

        public override string ToString()
        {
            return $"Character: {Kind}";
        }
    }
}