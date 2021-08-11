using Common;
using System.Collections.Generic;

namespace AST.Trees.Expressions.Untyped
{
    public class UntypedFunctionCallExpression : UntypedExpression
    {
        public UntypedFunctionCallExpression(DiagnosticInfo diag, string functionName, List<Expression> callArgs) : base(diag)
        {
            FunctionName = functionName;
            CallArguments = callArgs;
        }

        public string FunctionName { get; }

        public List<Expression> CallArguments { get; set; }

        public override string ToString()
        {
            return $"Untyped Function Call: {FunctionName}";
        }
    }
}
