using Common;

namespace AST.Trees.Expressions.Typed
{
    public class TypedVariableUseExpression : TypedExpression
    {
        public TypedVariableUseExpression(string variableName, TypeInfo typeInfo, UntypedScopeContext context, DiagnosticInfo diag) : base(diag, typeInfo, context)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }

        public override bool IsConstant()
        {
            return false;
        }

        public override string ToString()
        {
            return $"Typed Variable Use: {VariableName}";
        }
    }
}
