using System.Collections.Generic;
using Common.Lexing;

namespace AST.Miscs.Matching
{
    public class MatchingWholeGroup : MatchingGroup
    {
        public MatchingWholeGroup(IEnumerable<LexingElement> items) : base(items)
        {
        }

        public bool Check(List<LexingElement> elements)
        {
            if (elements.Count < Items.Count)
                return false;

            for (int i = 0; i < Items.Count; i++)
            {
                var current1 = Items[i];
                var current2 = elements[i];

                if (current1 != current2)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
