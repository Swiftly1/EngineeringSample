using Common;
using AST.Miscs;
using AST.Trees;
using Text2Abstraction.LexicalElements;
using System.Collections.Generic;
using Common.Lexing;
using AST.Miscs.Matching;

namespace AST.Builders
{
    public partial class ASTBuilder
    {
        private class NamespaceBuilder : MovableLookup
        {
            private readonly ErrorHandler _errors = new ErrorHandler();

            private string NamespaceName { get; }

            private DiagnosticInfo Diagnostics { get; }

            public NamespaceBuilder(GroupedLexicalElements item) : base(item.Elements)
            {
                NamespaceName = item.NamespaceName;
                Diagnostics = item.Diagnostics;
            }

            public Node Build()
            {
                do
                {
                    var fMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Type, LexingElement.Word, LexingElement.OpenParenthesis)
                        .Evaluate(TakeToEnd());

                    if (fMatcher.Success)
                    {
                        var ahead = TryGetAhead(fMatcher.Items.Count);
                        var result = TryMatchFunction(ahead.Items);
                    }
                } while (MoveNext());

                return new NamespaceNode(Diagnostics, NamespaceName);
            }

            private Result<FunctionNode> TryMatchFunction(List<LexElement> matched)
            {
                var result = GetTillClosed(LexingElement.OpenParenthesis, LexingElement.ClosedParenthesis);

                if (!result.Success)
                {
                    _errors.AddError(result.Message, _Current.Diagnostics);
                }

                var args = result.Data;

                var node = new FunctionNode(_Current.Diagnostics, (matched[2] as LexWord).Value);

                return new Result<FunctionNode>(node);
            }

            public new (bool Sucess, List<LexElement> Items) TryGetAhead(int count)
            {
                return base.TryGetAhead(count);
            }
        }
    }
}