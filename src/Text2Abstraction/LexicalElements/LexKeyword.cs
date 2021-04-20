using System;
using Common;
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
            return $"Keyword: {Value} of Kind: {Enum.GetName(typeof(LexingElement), Kind)}";
        }

        public static implicit operator string(LexKeyword l) => l.Value;
    }
}