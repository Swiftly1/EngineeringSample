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
                    var fMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Type, LexingElement.Word, LexingElement.OpenParenthesis);

                    if (fMatcher.Evaluate(TakeToEnd(1), out var fMatcherResult))
                    {
                        var ahead = TryGetAhead(fMatcherResult.Items.Count);
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
                    return new ResultDiag<Node>(_errors.DumpErrors().ToList());
                }

                return new ResultDiag<Node>(node);
            }

            private ResultDiag<UntypedFunctionNode> TryMatchFunction(List<LexElement> matched)
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

                var argsResult = ExtractArgs(result.Data);

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
                var body = bodyBuilder.Build();

                if (!body.Success)
                    return body.ToFailedResult<UntypedFunctionNode>();

                string desiredType = matched[1] as LexKeyword;

                var node = new UntypedFunctionNode(matched[2].Diagnostics, desiredType, functionName, body.Data, argsResult.Data);

                return new ResultDiag<UntypedFunctionNode>(node);
            }

            private Result<List<Argument>> ExtractArgs(List<LexElement> data)
            {
                var withoutComma = data.Where((x, i) => (i + 1) % 3 != 0).ToList();

                if (withoutComma.Count % 2 != 0)
                {
                    return Result<List<Argument>>.Error("");
                }

                var args = new List<Argument>();

                for (int i = 0; i < withoutComma.Count; i += 2)
                {
                    var arg = new Argument
                    {
                        TypeName = withoutComma[i] as LexKeyword,
                        Name = withoutComma[i + 1] as LexWord
                    };

                    args.Add(arg);
                }

                return Result<List<Argument>>.Ok(args);
            }

            public new (bool Sucess, List<LexElement> Items) TryGetAhead(int count, bool includeCurrent)
            {
                return base.TryGetAhead(count, includeCurrent);
            }
        }
    }
}