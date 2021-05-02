using AST.Trees.Expressions.Untyped;
using Common;

namespace AST.Trees.Statements
{
    public class AssignmentStatement : StatementNode
    {
        public AssignmentStatement(string name, UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            Name = name;
            Expression = expression;
        }

        public string Name { get; }

        public UntypedExpression Expression { get; }

        public override string ToString()
        {
            return $"Assign to '{Name}' value: '{Expression}'";
        }
    }
}
