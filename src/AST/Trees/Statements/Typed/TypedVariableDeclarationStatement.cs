using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Statements.Typed
{
    public class TypedVariableDeclarationStatement : StatementNode
    {
        public TypedVariableDeclarationStatement(string variableName, TypedExpression expression, TypeInfo typeinfo, UntypedScopeContext context, DiagnosticInfo diag) : base(diag)
        {
            VariableName = variableName;
            Expression = expression;
            TypeInfo = typeinfo;
            ScopeContext = context;
            Children.Add(expression);
        }

        public string VariableName { get; set; }

        public TypeInfo TypeInfo { get; set; }

        public TypedExpression Expression { get; set; }

        public UntypedScopeContext ScopeContext { get; set; }

        public override string ToString()
        {
            return $"TypedVariableDeclarationStatement: '{VariableName}'";
        }
    }
}
