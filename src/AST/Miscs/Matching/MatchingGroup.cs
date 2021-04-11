using System.Collections.Generic;
using System.Linq;
using Common.Lexing;

namespace AST.Miscs.Matching
{
    public class MatchingGroup
    {
        public MatchingGroup(IEnumerable<LexingElement> items)
        {
            Items = items.ToList();
        }

        public List<LexingElement> Items { get; }
    }
}
