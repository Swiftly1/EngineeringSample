using System.Collections.Generic;
using Common;
using Text2Abstraction.LexicalElements;

namespace AST
{
    public class GroupedLexicalElements
    {
        public GroupedLexicalElements(DiagnosticInfo diagnostics)
        {
            Diagnostics = diagnostics;
        }

        public DiagnosticInfo Diagnostics { get; }

        public string Namespace { get; set; }

        public List<LexElement> Elements { get; } = new List<LexElement>();

        public GroupedLexicalElements AddElement(LexElement element)
        {
            Elements.Add(element);
            return this;
        }

        public override string ToString()
        {
            return $"Namespace '{Namespace}' made of {Elements.Count} elements.";
        }
    }
}
