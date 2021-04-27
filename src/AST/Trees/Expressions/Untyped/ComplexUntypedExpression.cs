using Common;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Expressions.Untyped
{
    public class ComplexUntypedExpression : UntypedExpression
    {
        public ComplexUntypedExpression(UntypedExpression left, UntypedExpression right, LexCharacter @operator, DiagnosticInfo diag) : base(diag)
        {
            Left = left;
            Operator = @operator;
            Right = right;

            Children.Add(Left);
            Children.Add(Right);
        }

        public UntypedExpression Left { get; set; }

        public LexCharacter Operator { get; set; }

        public UntypedExpression Right { get; set; }

        public override string ToString()
        {
            return $"Complex Untyped: Left:{Left}; Right:{Right}; Operator:{Operator}";
        }
    }
}
