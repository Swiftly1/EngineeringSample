using Common;
using System;
using AST.Trees;
using System.Linq;
using Common.Lexing;
using AST.Miscs.Matching;
using AST.Trees.Statements;
using AST.Trees.Expressions;
using System.Collections.Generic;
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

            public Node Build()
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

                return bodyNode;
            }

            private ResultDiag<IfStatementNode> TryMatchIfStatement(List<LexElement> items)
            {
                var result = GetTillClosed(LexingElement.OpenParenthesis, LexingElement.ClosedParenthesis);

                if (!result.Success)
                {
                    return result.ToFailedResult<IfStatementNode>();
                }

                MoveNext();

                var bodyResult = GetTillClosed(LexingElement.OpenBracket, LexingElement.ClosedBracket);

                if (!bodyResult.Success)
                {
                    return bodyResult.ToFailedResult<IfStatementNode>();
                }

                // TODO: Complete IfStatement Matching
                throw new NotImplementedException();
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

            private ResultDiag<VariableDeclarationStatement> TryMatchVariableDeclaration(List<LexElement> items)
            {
                string typeName = items[0] as LexKeyword;
                var name = items[1] as LexWord;
                var type = TypeFacts.TypeName2TypeMapper[typeName];

                var skipped = TakeToEnd(1);
                var result = TryMatchExpression(skipped);

                if (!result.Success)
                    return result.ToFailedResult<VariableDeclarationStatement>();

                var vdn = new VariableDeclarationStatement(name, type, result.Data, name.Diagnostics);

                return new ResultDiag<VariableDeclarationStatement>(vdn);
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