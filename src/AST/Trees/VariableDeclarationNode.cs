using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees
{
    public class VariableDeclarationNode : Node
    {
        public VariableDeclarationNode(string variableName, TypeInfo declaredType, UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            DeclaredType = declaredType;
            Expression = expression;
        }

        public string VariableName { get; set; }

        public TypeInfo DeclaredType { get; set; }

        public UntypedExpression Expression { get; set; }

        public override string ToString()
        {
            return $"VariableDeclaration: {VariableName}";
        }
    }
}
