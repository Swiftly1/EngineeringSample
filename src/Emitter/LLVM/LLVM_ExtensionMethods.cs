using Common;
using System;

namespace Emitter.LLVM
{
    public static class LLVM_ExtensionMethods
    {
        public static string ToLLVMType(this TypeInfo t)
        {
            if (t == TypeFacts.GetBoolean())
            {
                return "i1";
            }
            else if (t == TypeFacts.GetInt32())
            {
                return "i32";
            }
            else if (t == TypeFacts.GetInt64())
            {
                return "i64";
            }
            else if (t == TypeFacts.GetVoid())
            {
                return "void";
            }
            else if (t == TypeFacts.GetDouble())
            {
                return "double";
            }
            else if (t == TypeFacts.GetString())
            {
                return "i8*";
            }
            else
            {
                throw new NotImplementedException($"Unknown type {t}");
            }
        }

        public static string OperatorConverter(this ExpressionOperator op)
        {
            return op switch
            {
                ExpressionOperator.Addition => "add",
                ExpressionOperator.Substraction => "sub",
                ExpressionOperator.Multiplication => "mul",
                ExpressionOperator.GreaterThan => "icmp sgt",
                ExpressionOperator.GreaterOrEqual => "icmp sge",
                ExpressionOperator.LessThan => "icmp slt",
                ExpressionOperator.LessOrEqual => "icmp sle",
                ExpressionOperator.Equal => "icmp equal",
                ExpressionOperator.NotEqual => "icmp ne",
                ExpressionOperator.Division => "sdiv",
                _ => throw new Exception($"Operator {op} not supported."),
            };
        }
    }
}
