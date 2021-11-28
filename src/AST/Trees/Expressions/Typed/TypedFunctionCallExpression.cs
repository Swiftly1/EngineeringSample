using Common;
using System.Collections.Generic;

namespace AST.Trees.Expressions.Typed
{
    public class TypedFunctionCallExpression : TypedExpression
    {
        public TypedFunctionCallExpression(DiagnosticInfo diag, string functionName, TypeInfo typeInfo, List<TypedExpression> callArgs, ScopeContext context)
            : base(diag, typeInfo, context)
        {
            FunctionName = functionName;
            CallArguments = callArgs;
        }

        public string FunctionName { get; }

        public List<TypedExpression> CallArguments { get; set; }

        public override bool IsConstant()
        {
            // TODO: does it even make sense?
            return false;
        }

        public override string ToString()
        {
            return $"Typed Function Call: {FunctionName}";
        }
    }
}
