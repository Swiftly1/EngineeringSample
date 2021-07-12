using Common;
using AST.Trees;
using System.Linq;
using Common.Lexing;
using AST.Miscs.Matching;
using AST.Trees.Statements;
using AST.Trees.Expressions;
using System.Collections.Generic;
using AST.Trees.Statements.Untyped;
using AST.Trees.Expressions.Untyped;
using Text2Abstraction.LexicalElements;

namespace AST.Builders
{
    public partial class ASTBuilder
    {
        private class BodyBuilder : MovableLookup
        {
            private readonly ErrorHandler _errors = new();

            private readonly DiagnosticInfo _Diagnostics;

            public BodyBuilder(List<LexElement> items, DiagnosticInfo diagnostic) : base(items)
            {
                _Diagnostics = diagnostic;
            }

            public ResultDiag<BodyNode> Build()
            {
                var bodyNode = new BodyNode(_Diagnostics);
                do
                {
                    var variableDeclarationMatcher =
                    MatcherUtils
                    .Match(LexingElement.Type, LexingElement.Word, LexingElement.Equal);

                    var assignStatementMatcher =
                    MatcherUtils
                    .Match(LexingElement.Word, LexingElement.Equal);

                    var ifStatementMatcher =
                    MatcherUtils
                    .Match(LexingElement.If);

                    if (variableDeclarationMatcher.Evaluate(TakeToEnd(), out var variableDeclarationMatcherResult))
                    {
                        var ahead = TryGetAhead(variableDeclarationMatcherResult.Items.Count, true);
                        var result = TryMatchVariableDeclaration(ahead.Items);

                        if (result.Success)
                            bodyNode.AddChild(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                    else if (assignStatementMatcher.Evaluate(TakeToEnd(), out var assignStatementMatcherResult))
                    {
                        var ahead = TryGetAhead(assignStatementMatcherResult.Items.Count, true);
                        var result = TryMatchVariableReAssignment(ahead.Items);

                        if (result.Success)
                            bodyNode.AddChild(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                    else if (ifStatementMatcher.Evaluate(TakeToEnd(), out var ifStatementMatcherResult))
                    {
                        var ahead = TryGetAhead(ifStatementMatcherResult.Items.Count);
                        var result = TryMatchIfStatement(ahead.Items);

                        if (result.Success)
                            bodyNode.AddChild(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                } while (MoveNext());

                if (_errors.DumpErrors().Any())
                {
                    return new ResultDiag<BodyNode>(_errors.DumpErrors().ToList());
                }

                return new ResultDiag<BodyNode>(bodyNode);
            }

            private ResultDiag<UntypedIfStatement> TryMatchIfStatement(List<LexElement> items)
            {
                var conditionResult = GetTillClosed(LexingElement.OpenParenthesis, LexingElement.ClosedParenthesis);

                if (!conditionResult.Success)
                {
                    return conditionResult.ToFailedResult<UntypedIfStatement>();
                }

                MoveNext();

                var bodyResult = GetTillClosed(LexingElement.OpenBracket, LexingElement.ClosedBracket);

                if (!bodyResult.Success)
                {
                    return bodyResult.ToFailedResult<UntypedIfStatement>();
                }

                var expressionBuilder = new ExpressionBuilder(conditionResult.Data);
                var expression = expressionBuilder.Build();

                if (!expression.Success)
                {
                    return expression.ToFailedResult<UntypedIfStatement>();
                }

                var trueBranch = new BodyBuilder(bodyResult.Data, items[0].Diagnostics).Build();

                if (!trueBranch.Success)
                {
                    return trueBranch.ToFailedResult<UntypedIfStatement>();
                }

                var node_without_else = new UntypedIfStatement(expression.Data, trueBranch.Data, null, items[0].Diagnostics);

                if (!CanMoveNext())
                {
                    return new ResultDiag<UntypedIfStatement>(node_without_else);
                }

                MoveNext();

                if (_Current.Kind != LexingElement.Else)
                {
                    MoveBehind();
                    return new ResultDiag<UntypedIfStatement>(node_without_else);
                }

                var elseDiagnostics = _Current.Diagnostics;
                MoveNext();

                var elseBodyResult = GetTillClosed(LexingElement.OpenBracket, LexingElement.ClosedBracket);

                if (!elseBodyResult.Success)
                {
                    return elseBodyResult.ToFailedResult<UntypedIfStatement>();
                }

                var falseBranch = new BodyBuilder(elseBodyResult.Data, elseDiagnostics).Build();

                if (!falseBranch.Success)
                {
                    return trueBranch.ToFailedResult<UntypedIfStatement>();
                }

                var node_with_two_branches = new UntypedIfStatement(expression.Data, trueBranch.Data, falseBranch.Data, items[0].Diagnostics);

                return new ResultDiag<UntypedIfStatement>(node_with_two_branches);
            }

            private ResultDiag<AssignmentStatement> TryMatchVariableReAssignment(List<LexElement> items)
            {
                var name = items[0] as LexWord;

                var skipped = TakeToEnd(1);
                var result = TryMatchExpression(skipped);

                if (!result.Success)
                    return result.ToFailedResult<AssignmentStatement>();

                var assignStatement = new AssignmentStatement(name.Value, result.Data, name.Diagnostics);

                return new ResultDiag<AssignmentStatement>(assignStatement);
            }

            private ResultDiag<UntypedVariableDeclarationStatement> TryMatchVariableDeclaration(List<LexElement> items)
            {
                string typeName = items[0] as LexKeyword;
                var name = items[1] as LexWord;

                var skipped = TakeToEnd(1);
                var result = TryMatchExpression(skipped);

                if (!result.Success)
                    return result.ToFailedResult<UntypedVariableDeclarationStatement>();

                var vdn = new UntypedVariableDeclarationStatement(name, typeName, result.Data, name.Diagnostics);

                return new ResultDiag<UntypedVariableDeclarationStatement>(vdn);
            }

            private ResultDiag<UntypedExpression> TryMatchExpression(List<LexElement> items)
            {
                // skip '='
                var expressionElements = items.TakeWhile(x => x.Kind != LexingElement.SemiColon).ToList();

                var builder = new ExpressionBuilder(expressionElements);
                var result = builder.Build();

                // Move pointer at the semicolon after expression
                TryGetAhead(expressionElements.Count + 1);
                return result;
            }

            public new(bool Sucess, List<LexElement> Items) TryGetAhead(int count, bool includeCurrent = false)
            {
                return base.TryGetAhead(count, includeCurrent);
            }
        }
    }
}