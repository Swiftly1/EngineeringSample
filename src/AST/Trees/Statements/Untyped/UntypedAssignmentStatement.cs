using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements.Untyped
{
    public class UntypedAssignmentStatement : StatementNode
    {
        public UntypedAssignmentStatement(string name, UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            Name = name;
            Expression = expression;
        }

        public string Name { get; }

        public UntypedExpression Expression { get; }

        public override string ToString()
        {
            return $"UntypedAssignmentStatement - '{Name}' value: '{Expression}'";
        }
    }
}
