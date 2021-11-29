using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements.Untyped
{
    public class UntypedVariableDeclarationStatement : StatementNode
    {
        public UntypedVariableDeclarationStatement(string variableName, string desiredType, UntypedExpression expression, ScopeContext context, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            DesiredType = desiredType;
            ScopeContext = context;
            Children.Add(expression);
        }

        public string VariableName { get; set; }

        public string DesiredType { get; set; }

        public UntypedExpression Expression => Children[0] as UntypedExpression;

        public ScopeContext ScopeContext { get; set; }

        public override string ToString()
        {
            return $"UntypedVariableDeclaration: '{VariableName}'";
        }
    }
}
