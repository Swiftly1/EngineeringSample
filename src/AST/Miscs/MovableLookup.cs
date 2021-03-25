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
            var originalIndex = _Index;

            var result = TryGetAhead(items.Length);

            if (!result.Sucess)
                return false;

            var equals = result.Items.Select(x => x.Kind).SequenceEqual(items);

            if (!equals)
                _Index = originalIndex;

            return equals;
        }

        protected bool MatchesThose(out List<LexElement> output, params LexingElement[] items)
        {
            var originalIndex = _Index;

            var result = TryGetAhead(items.Length);

            if (!result.Sucess)
            {
                output = new List<LexElement>();
                return false;
            }

            var equals = result.Items.Select(x => x.Kind).SequenceEqual(items);

            if (!equals)
                _Index = originalIndex;

            output = result.Items;
            return equals;
        }

        protected Result<List<LexElement>> GetTillClosed(LexingElement opening, LexingElement closing)
        {
            var openCounter = 0;

            if (_Current.Kind == opening)
                openCounter++;
            else
                return new Result<List<LexElement>>($"Expected element: '{opening}' around {_Current.Diagnostics.ToString()}.");

            var list = new List<LexElement>();

            while (openCounter > 0 && MoveNext())
            {
                if (_Current.Kind == opening)
                    openCounter++;
                else if (_Current.Kind == closing)
                    openCounter--;
                else
                {
                    list.Add(_Current);
                }
            }

            return new Result<List<LexElement>>(list);
        }
    }
}
