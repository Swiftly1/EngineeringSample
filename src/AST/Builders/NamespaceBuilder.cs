using Common;
using AST.Miscs;
using AST.Trees;
using Text2Abstraction.LexicalElements;
using System.Collections.Generic;
using Common.Lexing;
using System.Linq;
using System;

namespace AST.Builders
{
    public partial class ASTBuilder
    {
        private class NamespaceBuilder : MovableLookup
        {
            private readonly ErrorHandler _errors = new ErrorHandler();

            public string NamespaceName { get; set; }

            public NamespaceBuilder(GroupedLexicalElements item) : base(item.Elements)
            {
            }

            public Node Build()
            {
                do
                {
                    if (MatchesThose(
                        LexingElement.AccessibilityModifier,
                        LexingElement.Type,
                        LexingElement.Word,
                        LexingElement.OpenParenthesis,
                        LexingElement.ClosedParenthesis))
                    {
                        Console.WriteLine("matched");
                    }
                } while (MoveNext());

                return new NamespaceNode(NamespaceName);
            }

            public new (bool Sucess, List<LexElement> Items) TryGetAhead(int count)
            {
                return base.TryGetAhead(count);
            }
        }
    }
}