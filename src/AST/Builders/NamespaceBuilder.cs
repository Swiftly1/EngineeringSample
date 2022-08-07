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
using System;

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

                    var containerMatcher =
                        MatcherUtils
                        .Match(LexingElement.AccessibilityModifier, LexingElement.Container, LexingElement.Word, LexingElement.OpenBracket);

                    if (functionMatcher.Evaluate(TakeToEnd(), out var fMatcherResult))
                    {
                        var ahead = TryGetAhead(fMatcherResult.Items.Count, true);
                        var result = TryMatchFunction(ahead.Items, node.ScopeContext);

                        if (result.Success)
                            node.Children.Add(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                    else if (containerMatcher.Evaluate(TakeToEnd(), out var containerMatcherResult))
                    {
                        var ahead = TryGetAhead(containerMatcherResult.Items.Count, includeCurrent: true);
                        var result = TryMatchContainer(ahead.Items, node.ScopeContext);

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

            private ResultDiag<UntypedContainerNode> TryMatchContainer(List<LexElement> items, UntypedScopeContext parentScope)
            {
                var result = GetTillClosed(LexingElement.OpenBracket, LexingElement.ClosedBracket);
                if (!result.Success)
                {
                    return result.ToFailedResult<UntypedContainerNode>();
                }

                var accessModifier = items[0] as LexKeyword;
                var containerName = items[2] as LexWord;

                var fieldsResult = ExtractionHelpers.ExtractContainerFieldList(result.Data);

                if (!fieldsResult.Success)
                    return new ResultDiag<UntypedContainerNode>(Message.CreateError(fieldsResult.Message, containerName.Diagnostics));

                var node = new UntypedContainerNode
                (
                    items[1].Diagnostics,
                    containerName,
                    accessModifier,
                    new UntypedScopeContext(parentScope, $"container_{containerName.Value}"),
                    fieldsResult.Data
                );

                return new ResultDiag<UntypedContainerNode>(node);
            }

            private ResultDiag<UntypedFunctionNode> TryMatchFunction(List<LexElement> matched, UntypedScopeContext parentScope)
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

                var argsResult = ExtractionHelpers.ExtractFunctionParametersInfo(result.Data);

                if (!argsResult.Success)
                {
                    var txt = $"Arguments of function '{functionName}' should be made of `typeName argName` " +
                        "and comma if there are more arguments. " +
                        "e.g - 'int test', 'int a, int b'";

                    var message = Message.CreateError(txt, matched[2].Diagnostics);
                    _errors.AddMessage(message);
                    return new ResultDiag<UntypedFunctionNode>(message);
                }

                var context = new UntypedScopeContext(parentScope, $"function_{functionName}");

                foreach (var arg in argsResult.Data)
                {
                    context.DeclareVariable(new BasicVariableDescription(arg.Name, arg.TypeName));
                }

                var bodyBuilder = new BodyBuilder(bodyResult.Data, _Diagnostics);
                var body = bodyBuilder.Build(context);

                if (!body.Success)
                    return body.ToFailedResult<UntypedFunctionNode>();

                var accessModifier = matched[0] as LexKeyword;
                var desiredType = matched[1] as LexKeyword;

                var node = new UntypedFunctionNode
                (
                    matched[2].Diagnostics,
                    functionName,
                    desiredType.Value,
                    body.Data,
                    argsResult.Data,
                    desiredType.Diagnostics,
                    accessModifier,
                    context
                );

                return new ResultDiag<UntypedFunctionNode>(node);
            }
        }
    }
}