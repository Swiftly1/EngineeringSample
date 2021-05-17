﻿using System.Collections.Generic;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace Common
{
    public abstract class MovableLookup : Movable<LexElement>
    {
        protected MovableLookup(List<LexElement> Collection) : base(Collection)
        {
        }

        protected Result<List<LexElement>> GetTillClosed(LexingElement opening, LexingElement closing)
        {
            var openCounter = 0;

            if (_Current.Kind == opening)
                openCounter++;
            else
                return new Result<List<LexElement>>($"Expected element: '{opening}' around {_Current.Diagnostics}.", _Current.Diagnostics);

            var list = new List<LexElement>();

            while (openCounter > 0 && MoveNext())
            {
                if (_Current.Kind == opening)
                {
                    openCounter++;
                }
                else if (_Current.Kind == closing)
                {
                    openCounter--;
                }
                else
                {
                    list.Add(_Current);
                }
            }

            return new Result<List<LexElement>>(list);
        }
    }
}
