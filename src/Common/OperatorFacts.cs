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
            new(TypeFacts.GetInt32(), ExpressionOperator.Division, TypeFacts.GetInt32(), TypeFacts.GetDouble()),

            new(TypeFacts.GetInt32(), ExpressionOperator.GreaterThan, TypeFacts.GetInt32(), TypeFacts.GetBoolean()),
            new(TypeFacts.GetInt32(), ExpressionOperator.GreaterOrEqual, TypeFacts.GetInt32(), TypeFacts.GetBoolean()),

            new(TypeFacts.GetInt32(), ExpressionOperator.LessThan, TypeFacts.GetInt32(), TypeFacts.GetBoolean()),
            new(TypeFacts.GetInt32(), ExpressionOperator.LessOrEqual, TypeFacts.GetInt32(), TypeFacts.GetBoolean()),
        };

        public static TypeOperatorDefinition TryMatch(TypeInfo left, TypeInfo right, ExpressionOperator @operator)
        {
            return OperatorsBetweenTypes.FirstOrDefault(x => x.Left.Name == left.Name && x.Right.Name == right.Name && x.Operator == @operator);
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
                case LexingElement.GreaterThan:
                        return ExpressionOperator.GreaterThan;
                case LexingElement.GreaterOrEqual:
                        return ExpressionOperator.GreaterOrEqual;
                case LexingElement.LessThan:
                        return ExpressionOperator.LessThan;
                case LexingElement.LessOrEqual:
                        return ExpressionOperator.LessOrEqual;
                default:
                    throw new Exception($"Operator '{kind}' is not defined");
            };
        }
    }
}
