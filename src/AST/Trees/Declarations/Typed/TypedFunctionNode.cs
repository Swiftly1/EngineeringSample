using Common;
using System.Linq;
using AST.Trees.Miscs;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Declarations.Typed
{
    public class TypedFunctionNode : ScopeableNode
    {
        public TypedFunctionNode
        (
            DiagnosticInfo diag,
            string name,
            Node body,
            List<TypedArgument> args,
            TypeInfo type,
            UntypedScopeContext scopeContext,
            DiagnosticInfo typeDiag,
            LexKeyword accessModifier
        ) : base(diag, scopeContext)
        {
            Name = name;
            Type = type;
            Arguments = args;
            Children.Add(body);
            TypeDiagnostics = typeDiag;
            AccessibilityModifier = accessModifier;
        }

        public string Name { get; }

        public Node Body => Children[0];

        public TypeInfo Type { get; set; }

        public List<TypedArgument> Arguments { get; set; } = new List<TypedArgument>();

        public DiagnosticInfo TypeDiagnostics { get; set; }

        public LexKeyword AccessibilityModifier { get; set; }

        public override string ToString()
        {
            var args = string.Join(", ", Arguments.Select(x => $"({x.TypeInfo.Name})"));

            args = args.Length > 0 ? $"Args: {args}." : "No args.";

            return $"TypedFunction: '{Name}'({Type.Name}). {args}";
        }
    }
}
