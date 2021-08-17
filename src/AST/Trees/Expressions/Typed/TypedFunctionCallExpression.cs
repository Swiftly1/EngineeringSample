using Common;
using System.Collections.Generic;
using AST.Trees.Expressions.Typed;

namespace AST.Trees.Expressions.Untyped
{
    public class TypedFunctionCallExpression : TypedExpression
    {
        public TypedFunctionCallExpression(DiagnosticInfo diag, string functionName, TypeInfo typeInfo, List<Expression> callArgs) : base(diag, typeInfo)
        {
            FunctionName = functionName;
            CallArguments = callArgs;
        }

        public string FunctionName { get; }

        public List<Expression> CallArguments { get; set; }

        public override string ToString()
        {
            return $"Typed Function Call: {FunctionName}";
        }
    }
}
