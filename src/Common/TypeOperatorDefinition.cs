namespace Common
{
    public class TypeOperatorDefinition
    {
        public TypeOperatorDefinition(TypeInfo left, ExpressionOperator @operator, TypeInfo right, TypeInfo resultType)
        {
            Left = left;
            Operator = @operator;
            Right = right;
            ResultType = resultType;
        }

        public TypeInfo Left { get; }

        public ExpressionOperator @Operator { get; }

        public TypeInfo Right { get; }

        public TypeInfo ResultType { get; }
    }
}
