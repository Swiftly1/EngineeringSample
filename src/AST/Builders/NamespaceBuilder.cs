using Common;
using AST.Miscs;
using AST.Trees;
using System.Linq;
using Common.Lexing;
using AST.Trees.Miscs;
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

            public ResultDiag<Node> TryBuild()
            {
                var node = new NamespaceNode(_Diagnostics, NamespaceName);

                do
                {
                    var functionMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Type, LexingElement.Word, LexingElement.OpenParenthesis);

                    if (functionMatcher.Evaluate(TakeToEnd(), out var fMatcherResult))
                    {
                        var ahead = TryGetAhead(fMatcherResult.Items.Count, true);
                        var result = TryMatchFunction(ahead.Items, node.ScopeContext);

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
                    return new ResultDiag<Node>(_errors.DumpErrors().ToList());
                }

                return new ResultDiag<Node>(node);
            }

            private ResultDiag<UntypedFunctionNode> TryMatchFunction(List<LexElement> matched, ScopeContext parentScope)
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

                string functionName = matched[2] as LexWord;

                var argsResult = FunctionHelpers.ExtractFunctionParametersInfo(result.Data);

                if (!argsResult.Success)
                {
                    var txt = $"Arguments of function '{functionName}' should be made of `typeName argName` " +
                        $"and comma if there are more arguments. " +
                        "e.g - 'int test', 'int a, int b'";

                    var message = Message.CreateError(txt, matched[2].Diagnostics);
                    _errors.AddMessage(message);
                    return new ResultDiag<UntypedFunctionNode>(message);
                }

                var bodyBuilder = new BodyBuilder(bodyResult.Data, _Diagnostics);
                var body = bodyBuilder.Build(parentScope);

                if (!body.Success)
                    return body.ToFailedResult<UntypedFunctionNode>();

                var accessModifier = matched[0] as LexKeyword;
                var desiredType = matched[1] as LexKeyword;
                var context = new ScopeContext(parentScope);

                var node = new UntypedFunctionNode
                (
                    matched[2].Diagnostics,
                    functionName,
                    desiredType.Value,
                    body.Data,
                    argsResult.Data,
                    desiredType.Diagnostics,
                    accessModifier.Diagnostics,
                    context
                );

                return new ResultDiag<UntypedFunctionNode>(node);
            }
        }
    }
}