using System.Collections.Generic;
using AST.Miscs.Matching;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace Common
{
    public abstract class MovableLookup : Movable<LexElement>
    {
        public MovableLookup(List<LexElement> Collection) : base(Collection)
        {
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
