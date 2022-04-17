using Common;
using AST.Trees.Expressions.Untyped;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Statements.Typed
{
    public class TypedIfStatement : StatementNode
    {
        public TypedIfStatement(TypedExpression condition, BodyNode branchTrue, BodyNode branchFalse, DiagnosticInfo diag) : base(diag)
        {
            Condition = condition;

            this.AddChild(branchTrue);

            // TODO: It's not the nicest solution
            // because we aren't sure whether there was just empty block or it was no provided at all
            // but it simplifies stuff a little bit 
            this.AddChild(branchFalse ?? new BodyNode(diag, null));
        }

        public TypedExpression Condition { get; }

        public BodyNode BranchTrue => Children[0] as BodyNode;

        public BodyNode BranchFalse => Children[1] as BodyNode;

        public override string ToString()
        {
            return $"TypedIf - {Condition}";
        }
    }
}
