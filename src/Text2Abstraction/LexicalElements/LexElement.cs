namespace Text2Abstraction.LexicalElements
{
    public class LexElement
    {
        private readonly DiagnosticInfo _diagnostic;

        public LexingElement Kind { get; }

        public LexElement(LexingElement kind, DiagnosticInfo diagnostic)
        {
            _diagnostic = diagnostic;
            Kind = kind;
        }

        public override string ToString()
        {
            return "LexElement";
        }
    }
}