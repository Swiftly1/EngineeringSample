using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements
{
    public class UntypedIfStatement : StatementNode
    {
        public UntypedIfStatement(UntypedExpression condition, BodyNode branchTrue, BodyNode branchFalse, DiagnosticInfo diag) : base(diag)
        {
            Condition = condition;
            BranchTrue = branchTrue;
            // TODO: It's not the nicest solution
            // because we aren't sure whether there was just empty block or it was no provided at all
            // but it simplifies code a little bit 
            BranchFalse = branchFalse ?? new BodyNode(diag);

            this.AddChild(BranchTrue);
            this.AddChild(BranchFalse);
        }

        public UntypedExpression Condition { get; }

        public BodyNode BranchTrue { get; }

        public BodyNode BranchFalse { get; }

        public override string ToString()
        {
            return $"If - {Condition}";
        }
    }
}
