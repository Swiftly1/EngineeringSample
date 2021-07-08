using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements
{
    public class VariableDeclarationStatement : StatementNode
    {
        public VariableDeclarationStatement(string variableName, string desiredType, UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            DesiredType = desiredType;
            Expression = expression;
            Children.Add(expression);
        }

        public string VariableName { get; set; }

        public string DesiredType { get; set; }

        public Node Expression { get; set; }

        public override string ToString()
        {
            return $"VariableDeclaration: '{VariableName}'";
        }
    }
}
