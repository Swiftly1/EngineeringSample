using System;
using Common;
using Common.Lexing;

namespace Text2Abstraction.LexicalElements
{
    public class LexElement
    {
        public DiagnosticInfo Diagnostics;

        public LexingElement Kind { get; }

        public LexElement(LexingElement kind, DiagnosticInfo diagnostic)
        {
            Diagnostics = diagnostic;
            Kind = kind;
        }

        public override string ToString()
        {
            return $"LexElement {Enum.GetName(typeof(LexingElement), Kind)}";
        }
    }
}