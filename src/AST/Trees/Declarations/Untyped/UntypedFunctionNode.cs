using Common;
using AST.Trees.Miscs;
using System.Collections.Generic;

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
            DiagnosticInfo accessModDiag
        ) : base(diag, new ScopeContext())
        {
            // TODO: ScopeContext should be received from parent Namespace
            // and that ScopeContext should contain info about other functions / classes 
            // within that namespace
            Name = name;
            DesiredType = desiredType;
            Body = body;
            Arguments = args;
            Children.Add(Body);
            TypeDiagnostics = typeDiag;
            AccessibilityModifierDiagnostics = accessModDiag;
        }

        public string Name { get; }

        public string DesiredType { get; set; }

        public BodyNode Body { get; set; }

        public List<Argument> Arguments { get; set; } = new List<Argument>();

        public DiagnosticInfo TypeDiagnostics { get; set; }

        public DiagnosticInfo AccessibilityModifierDiagnostics { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
