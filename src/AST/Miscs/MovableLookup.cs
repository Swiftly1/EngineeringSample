using System.Collections.Generic;
using System.Linq;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace Common
{
    public abstract class MovableLookup : Movable<LexElement>
    {
        public MovableLookup(List<LexElement> Collection) : base(Collection)
        {
        }

        protected bool MatchesThose(params LexingElement[] items)
        {
            var result = TryGetAhead(items.Length);

            if (!result.Sucess)
                return false;

            return result.Items.Select(x => x.Kind).SequenceEqual(items);
        }
    }
}
