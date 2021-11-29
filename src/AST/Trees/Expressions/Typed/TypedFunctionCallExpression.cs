using Common;
using System.Linq;
using System.Collections.Generic;

namespace AST.Trees.Expressions.Typed
{
    public class TypedFunctionCallExpression : TypedExpression
    {
        public TypedFunctionCallExpression(DiagnosticInfo diag, string functionName, TypeInfo typeInfo, List<TypedExpression> callArgs, UntypedScopeContext context)
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
            var args = string.Join(", ", CallArguments.Select(x => $"({x.TypeInfo.Name})"));

            args = args.Length > 0 ? $"Args: {args}." : "No args.";

            return $"Typed Function Call: '{FunctionName}'({TypeInfo.Name}). {args}";
        }
    }
}
