using Common;
using AST.Trees.Miscs;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Declarations.Untyped
{
    public class UntypedFunctionNode : ScopeableNode
    {
        public UntypedFunctionNode
        (
            DiagnosticInfo diag,
            string name,
            string desiredType,
            BodyNode body,
            List<Argument> args,
            DiagnosticInfo typeDiag,
            LexKeyword accessModifier,
            UntypedScopeContext context
        ) : base(diag, context)
        {
            Name = name;
            DesiredType = desiredType;
            Body = body;
            Arguments = args;
            Children.Add(Body);
            TypeDiagnostics = typeDiag;
            AccessibilityModifier = accessModifier;
        }

        public string Name { get; }

        public string DesiredType { get; set; }

        public BodyNode Body { get; set; }

        public List<Argument> Arguments { get; set; } = new List<Argument>();

        public DiagnosticInfo TypeDiagnostics { get; set; }

        public LexKeyword AccessibilityModifier { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
