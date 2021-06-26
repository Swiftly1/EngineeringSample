using System;
using System.Collections.Generic;
using System.Linq;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace AST.Miscs.Matching
{
    public class Matcher
    {
        public Matcher(params LexingElement[] items)
        {
            Requirements.Add(new MatchingWholeGroup(items));
        }

        private List<MatchingGroup> Requirements { get; } = new List<MatchingGroup>();

        public Matcher Then(params LexingElement[] items)
        {
            Requirements.Add(new MatchingWholeGroup(items));
            return this;
        }

        public Matcher ThenAnyOf(params LexingElement[] items)
        {
            Requirements.Add(new MatchingAnyFromGroup(items));
            return this;
        }

        public bool Evaluate(List<LexElement> lexElements, out EvaluationResult result)
        {
            var items = new List<LexElement>();
            var enumerator = lexElements.GetEnumerator();
            enumerator.MoveNext();

            var hasEnded = false;

            foreach (var requirement in Requirements)
            {
                if (hasEnded)
                {
                    result = new EvaluationResult(false, enumerator.Current);
                    return false;
                }

                var satisfies = Verify(requirement, enumerator);

                if (satisfies.Success)
                {
                    items.AddRange(satisfies.Verified);
                    if (!enumerator.MoveNext())
                    {
                        hasEnded = true;
                    }
                }
                else
                {
                    result = new EvaluationResult(false, enumerator.Current);
                    return false;
                }
            }

            result = new EvaluationResult(true, items);
            return true;
        }

        private (bool Success, List<LexElement> Verified) Verify(MatchingGroup requirement, List<LexElement>.Enumerator enumerator)
        {
            if (requirement is MatchingAnyFromGroup any)
            {
                return (any.Check(enumerator.Current.Kind), new List<LexElement> { enumerator.Current });
            }
            else if (requirement is MatchingWholeGroup wg)
            {
                // TODO:
                // it's ugly
                var count = wg.Items.Count;
                var items = new List<LexElement>();

                do
                {
                    items.Add(enumerator.Current);
                } while (enumerator.MoveNext() && items.Count < count);

                if (items.Count != count)
                    return (false, new List<LexElement>());

                return (wg.Check(items.Select(x => x.Kind).ToList()), items);
            }
            else
            {
                throw new NotImplementedException($"Requirement of type '{requirement.GetType()}' is not supported");
            }
        }
    }
}
