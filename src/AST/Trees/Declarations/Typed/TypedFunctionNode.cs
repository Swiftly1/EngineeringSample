using Common;
using System.Linq;
using AST.Trees.Miscs;
using System.Collections.Generic;

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
            ScopeContext scopeContext,
            DiagnosticInfo typeDiag,
            DiagnosticInfo accessModDiag
        ) : base(diag, scopeContext)
        {
            Name = name;
            Type = type;
            Arguments = args;
            Children.Add(body);
            TypeDiagnostics = typeDiag;
            AccessibilityModifierDiagnostics = accessModDiag;
        }

        public string Name { get; }

        public Node Body => Children[0];

        public TypeInfo Type { get; set; }

        public List<TypedArgument> Arguments { get; set; } = new List<TypedArgument>();

        public DiagnosticInfo TypeDiagnostics { get; set; }

        public DiagnosticInfo AccessibilityModifierDiagnostics { get; set; }

        public override string ToString()
        {
            var args = string.Join(", ", Arguments.Select(x => $"({x.Type.Name})"));

            args = args.Length > 0 ? $"Args: {args}." : "No args.";

            return $"TypedFunction: '{Name}'({Type.Name}). {args}";
        }
    }
}
