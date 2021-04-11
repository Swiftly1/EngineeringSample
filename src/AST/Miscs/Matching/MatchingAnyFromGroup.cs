using System.Collections.Generic;
using Common.Lexing;

namespace AST.Miscs.Matching
{
    public class MatchingAnyFromGroup : MatchingGroup
    {
        public MatchingAnyFromGroup(IEnumerable<LexingElement> items) : base(items)
        {
        }

        public bool Check(LexingElement element)
        {
            return Items.Contains(element);
        }
    }
}
