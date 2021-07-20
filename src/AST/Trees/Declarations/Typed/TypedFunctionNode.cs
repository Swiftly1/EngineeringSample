using Common;

namespace AST.Trees.Declarations.Typed
{
    public class TypedFunctionNode : ScopeableNode
    {
        public TypedFunctionNode
        (
            DiagnosticInfo diag,
            string name,
            Node body,
            TypeInfo type,
            ScopeContext scopeContext,
            DiagnosticInfo typeDiag,
            DiagnosticInfo accessModDiag
        ) : base(diag, scopeContext)
        {
            Name = name;
            Type = type;
            Children.Add(body);
            TypeDiagnostics = typeDiag;
            AccessibilityModifierDiagnostics = accessModDiag;
        }

        public string Name { get; }

        public Node Body => Children[0];

        public TypeInfo Type { get; set; }

        public DiagnosticInfo TypeDiagnostics { get; set; }

        public DiagnosticInfo AccessibilityModifierDiagnostics { get; set; }

        public override string ToString()
        {
            return $"Function: '{Name}'";
        }
    }
}
