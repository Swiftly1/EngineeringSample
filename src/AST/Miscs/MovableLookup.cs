using Common.Lexing;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace Common
{
    public abstract class MovableLookup : Movable<LexElement>
    {
        protected MovableLookup(List<LexElement> Collection) : base(Collection)
        {
        }

        protected ResultDiag<List<LexElement>> GetTillClosed(LexingElement opening, LexingElement closing)
        {
            var openCounter = 0;

            if (_Current.Kind == opening)
                openCounter++;
            else
                return new ResultDiag<List<LexElement>>($"Expected element: '{opening}' around {_Current.Diagnostics}.", _Current.Diagnostics);

            var list = new List<LexElement>();

            while (openCounter > 0 && MoveNext())
            {
                if (_Current.Kind == opening)
                {
                    openCounter++;

                    if (openCounter > 1)
                    {
                        list.Add(_Current);
                    }
                }
                else if (_Current.Kind == closing)
                {
                    if (openCounter > 1)
                    {
                        list.Add(_Current);
                    }

                    openCounter--;   
                }
                else
                {
                    list.Add(_Current);
                }
            }

            return new ResultDiag<List<LexElement>>(list);
        }
    }
}
