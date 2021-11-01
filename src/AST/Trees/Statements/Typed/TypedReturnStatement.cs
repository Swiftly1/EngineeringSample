using Common;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Statements.Typed
{
    public class TypedReturnStatement : StatementNode
    {
        public TypedReturnStatement(TypedExpression expression, DiagnosticInfo diag) : base(diag)
        {
            Children.Add(expression);
        }

        public TypedExpression ReturnExpression => Children[0] as TypedExpression;

        public override string ToString()
        {
            return $"TypedReturnStatement: '{ReturnExpression}'";
        }
    }
}
