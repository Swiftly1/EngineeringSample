using Common;
using AST.Trees;
using Text2Abstraction.LexicalElements;
using System.Collections.Generic;
using Common.Lexing;
using AST.Miscs.Matching;
using System;

namespace AST.Builders
{
    public partial class ASTBuilder
    {
        private class BodyBuilder : MovableLookup
        {
            private readonly ErrorHandler _errors = new ErrorHandler();

            public BodyBuilder(List<LexElement> items) : base(items)
            {
            }

            public Node Build()
            {
                return new BodyNode(new DiagnosticInfo(0,0,'c'));
                throw new NotImplementedException();

                do
                {
                    var fMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Type, LexingElement.Word, LexingElement.OpenParenthesis)
                        .Evaluate(TakeToEnd());

                    if (fMatcher.Success)
                    {
                        var ahead = TryGetAhead(fMatcher.Items.Count);
                    }
                } while (MoveNext());
            }

            public new(bool Sucess, List<LexElement> Items) TryGetAhead(int count)
            {
                return base.TryGetAhead(count);
            }
        }
    }
}