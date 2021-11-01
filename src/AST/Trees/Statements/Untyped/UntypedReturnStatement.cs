using Common;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Statements.Untyped
{
    public class UntypedReturnStatement : StatementNode
    {
        public UntypedReturnStatement(UntypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            Children.Add(expression);
        }

        public UntypedExpression ReturnExpression => Children[0] as UntypedExpression;

        public override string ToString()
        {
            return $"UntypedReturnStatement: '{ReturnExpression}'";
        }
    }
}
