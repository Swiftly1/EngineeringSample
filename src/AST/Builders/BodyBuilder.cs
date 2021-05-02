using Common;
using AST.Trees;
using Text2Abstraction.LexicalElements;
using System.Collections.Generic;
using Common.Lexing;
using AST.Miscs.Matching;
using AST.Trees.Expressions.Untyped;
using System.Linq;
using AST.Trees.Expressions;
using AST.Trees.Statements;

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
                    .Match(LexingElement.Type, LexingElement.Word, LexingElement.Equal)
                    .Evaluate(TakeToEnd());

                    var assignStatementMatcher =
                    MatcherUtils
                    .Match(LexingElement.Word, LexingElement.Equal)
                    .Evaluate(TakeToEnd());

                    if (variableDeclarationMatcher.Success)
                    {
                        var ahead = TryGetAhead(variableDeclarationMatcher.Items.Count, true);
                        var result = TryMatchVariableDeclaration(ahead.Items);

                        if (result.Success)
                            bodyNode.AddChild(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                    else if (assignStatementMatcher.Success)
                    {
                        var ahead = TryGetAhead(assignStatementMatcher.Items.Count, true);
                        var result = TryMatchVariableReAssignment(ahead.Items);

                        if (result.Success)
                            bodyNode.AddChild(result.Data);
                        else
                            _errors.AddMessages(result.Messages);
                    }
                } while (MoveNext());

                return bodyNode;
            }

            private Result<AssignmentStatement> TryMatchVariableReAssignment(List<LexElement> items)
            {
                var name = items[0] as LexWord;

                var skipped = TakeToEnd(1);
                var result = TryMatchExpression(skipped);

                if (!result.Success)
                    return result.ToFailedResult<AssignmentStatement>();

                var assignStatement = new AssignmentStatement(name.Value, result.Data, name.Diagnostics);

                return new Result<AssignmentStatement>(assignStatement);
            }

            private Result<VariableDeclarationStatement> TryMatchVariableDeclaration(List<LexElement> items)
            {
                string typeName = items[0] as LexKeyword;
                var name = items[1] as LexWord;
                var type = TypeFacts.TypeName2TypeMapper[typeName];

                var skipped = TakeToEnd(1);
                var result = TryMatchExpression(skipped);

                if (!result.Success)
                    return result.ToFailedResult<VariableDeclarationStatement>();

                var vdn = new VariableDeclarationStatement(name, type, result.Data, name.Diagnostics);

                return new Result<VariableDeclarationStatement>(vdn);
            }

            private Result<UntypedExpression> TryMatchExpression(List<LexElement> items)
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