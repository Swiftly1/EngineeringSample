using Common;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Miscs
{
    public class GroupedLexicalElements
    {
        public GroupedLexicalElements(DiagnosticInfo diagnostics)
        {
            Diagnostics = diagnostics;
        }

        public DiagnosticInfo Diagnostics { get; }

        public string NamespaceName { get; set; }

        public List<LexElement> Elements { get; } = new List<LexElement>();

        public GroupedLexicalElements AddElement(LexElement element)
        {
            Elements.Add(element);
            return this;
        }

        public override string ToString()
        {
            return $"Namespace '{NamespaceName}' made of {Elements.Count} elements.";
        }
    }
}
