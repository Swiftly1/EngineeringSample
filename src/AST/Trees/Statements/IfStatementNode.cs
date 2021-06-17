using Common;
using AST.Trees.Expressions;

namespace AST.Trees.Statements
{
    public class IfStatementNode : StatementNode
    {
        public IfStatementNode(Expression condition, BodyNode branchTrue, BodyNode branchFalse, DiagnosticInfo diag) : base(diag)
        {
            Condition = condition;
            BranchTrue = branchTrue;
            BranchFalse = branchFalse;
        }

        public Expression Condition { get; set; }

        public BodyNode BranchTrue { get; set; }

        public BodyNode BranchFalse { get; set; }

        public override string ToString()
        {
            return $"If - {Condition}";
        }
    }
}
