using System;
using Common.Lexing;

namespace Common
{
    public static class OperatorFacts
    {
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
