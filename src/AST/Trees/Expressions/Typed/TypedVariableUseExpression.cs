using Common;

namespace AST.Trees.Expressions.Typed
{
    public class TypedVariableUseExpression : TypedExpression
    {
        public TypedVariableUseExpression(string variableName, bool isConstant, TypeInfo typeInfo, UntypedScopeContext context, DiagnosticInfo diag) : base(diag, typeInfo, context)
        {
            VariableName = variableName;
            _isConstant = isConstant;
        }

        private bool _isConstant { get; }

        public string VariableName { get; }

        public override bool IsConstant()
        {
            return _isConstant;
        }

        public override string ToString()
        {
            return $"Typed Variable Use: {VariableName}";
        }
    }
}
