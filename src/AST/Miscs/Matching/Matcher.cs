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

        public EvaluationResult Evaluate(List<LexElement> lexElements)
        {
            var items = new List<LexElement>();
            var enumerator = lexElements.GetEnumerator();
            enumerator.MoveNext();

            var hasEnded = false;

            foreach (var requirement in Requirements)
            {
                if (hasEnded)
                {
                    return new EvaluationResult(false, enumerator.Current);
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
                    return new EvaluationResult(false, enumerator.Current);
                }
            }

            return new EvaluationResult(true, items);
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
            else throw new NotImplementedException($"Requirement of type '{requirement.GetType()}' is not supported");
        }
    }
}
