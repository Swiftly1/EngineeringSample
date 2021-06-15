using Common;
using AST.Miscs;
using AST.Trees;
using System.Linq;
using Common.Lexing;
using AST.Miscs.Matching;
using System.Collections.Generic;
using AST.Trees.Declarations.Untyped;
using Text2Abstraction.LexicalElements;

namespace AST.Builders
{
    public partial class ASTBuilder
    {
        private class NamespaceBuilder : MovableLookup
        {
            private readonly ErrorHandler _errors = new();

            private string NamespaceName { get; }

            private DiagnosticInfo _Diagnostics { get; }

            public NamespaceBuilder(GroupedLexicalElements item) : base(item.Elements)
            {
                NamespaceName = item.NamespaceName;
                _Diagnostics = item.Diagnostics;
            }

            public Result<Node> TryBuild()
            {
                var node = new NamespaceNode(_Diagnostics, NamespaceName);
                do
                {
                    var fMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Type, LexingElement.Word, LexingElement.OpenParenthesis)
                        .Evaluate(TakeToEnd(1));

                    if (fMatcher.Success)
                    {
                        var ahead = TryGetAhead(fMatcher.Items.Count);
                        var result = TryMatchFunction(ahead.Items);

                        if (result.Success)
                            node.Children.Add(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                    else
                    {
                        _errors.AddMessage(Message.CreateError($"Unexpected element {_Current}.", _Current.Diagnostics));
                    }
                } while (MoveNext());

                if (_errors.DumpErrors().Any())
                {
                    return new Result<Node>(_errors.DumpErrors().ToList());
                }

                return new Result<Node>(node);
            }

            private Result<UntypedFunctionNode> TryMatchFunction(List<LexElement> matched)
            {
                var result = GetTillClosed(LexingElement.OpenParenthesis, LexingElement.ClosedParenthesis);

                if (!result.Success)
                {
                    return result.ToFailedResult<UntypedFunctionNode>();
                }

                MoveNext();

                var bodyResult = GetTillClosed(LexingElement.OpenBracket, LexingElement.ClosedBracket);

                if (!bodyResult.Success)
                {
                    _errors.AddMessages(bodyResult.Messages);
                    return bodyResult.ToFailedResult<UntypedFunctionNode>();
                }

                var args = result.Data;
                string functionName = matched[2] as LexWord;
                var bodyBuilder = new BodyBuilder(bodyResult.Data, _Diagnostics);
                var body = bodyBuilder.Build();
                var node = new UntypedFunctionNode(matched[2].Diagnostics, functionName, body);

                return new Result<UntypedFunctionNode>(node);
            }

            public new (bool Sucess, List<LexElement> Items) TryGetAhead(int count, bool includeCurrent)
            {
                return base.TryGetAhead(count, includeCurrent);
            }
        }
    }
}