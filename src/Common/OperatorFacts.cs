using System;
using System.Linq;
using Common.Lexing;
using System.Collections.Generic;

namespace Common
{
    public static class OperatorFacts
    {
        public static List<TypeOperatorDefinition> OperatorsBetweenTypes = new()
        {
            new(TypeFacts.GetInt32(), ExpressionOperator.Addition, TypeFacts.GetInt32(), TypeFacts.GetInt32()),
            new(TypeFacts.GetInt32(), ExpressionOperator.Substraction, TypeFacts.GetInt32(), TypeFacts.GetInt32()),
            new(TypeFacts.GetInt32(), ExpressionOperator.Multiplication, TypeFacts.GetInt32(), TypeFacts.GetInt32()),
            new(TypeFacts.GetInt32(), ExpressionOperator.Division, TypeFacts.GetInt32(), TypeFacts.GetDouble())
        };

        public static TypeOperatorDefinition TryMatch(TypeInfo left, TypeInfo right)
        {
            return OperatorsBetweenTypes.FirstOrDefault(x => x.Left.Name == left.Name && x.Right.Name == right.Name);
        }

        public static ExpressionOperator Convert(LexingElement kind)
        {
            switch (kind)
            {
                case LexingElement.Star:
                        return ExpressionOperator.Multiplication;
                case LexingElement.Slash:
                    return ExpressionOperator.Division;
                case LexingElement.Plus:
                        return ExpressionOperator.Addition;
                case LexingElement.Minus:
                        return ExpressionOperator.Substraction;
                default:
                    throw new Exception("Operator '{s}' is not defined");
            };
        }
    }
}
