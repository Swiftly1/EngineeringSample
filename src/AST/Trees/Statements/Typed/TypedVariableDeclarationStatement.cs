using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Statements.Typed
{
    public class TypedVariableDeclarationStatement : StatementNode
    {
        public TypedVariableDeclarationStatement(string variableName, TypedExpression expression, TypeInfo typeinfo, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            Expression = expression;
            TypeInfo = typeinfo;
            Children.Add(expression);
        }

        public string VariableName { get; set; }

        public TypeInfo TypeInfo { get; set; }

        public TypedExpression Expression { get; set; }

        public override string ToString()
        {
            return $"TypedVariableDeclaration: '{VariableName}'";
        }
    }
}
