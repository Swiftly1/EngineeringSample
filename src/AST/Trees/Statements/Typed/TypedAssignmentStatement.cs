using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Statements.Typed
{
    public class TypedAssignmentStatement : StatementNode
    {
        public TypedAssignmentStatement(string name, TypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            Name = name;
            Expression = expression;
        }

        public string Name { get; }

        public TypedExpression Expression { get; }

        public override string ToString()
        {
            return $"TypedAssignmentStatement - '{Name}' value: '{Expression}'";
        }
    }
}
