using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements.Untyped
{
    public class UntypedVariableDeclarationStatement : StatementNode
    {
        public UntypedVariableDeclarationStatement(string variableName, string desiredType, UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            DesiredType = desiredType;
            Children.Add(expression);
        }

        public string VariableName { get; set; }

        public string DesiredType { get; set; }

        public UntypedExpression Expression => Children[0] as UntypedExpression;

        public override string ToString()
        {
            return $"VariableDeclaration: '{VariableName}'";
        }
    }
}
