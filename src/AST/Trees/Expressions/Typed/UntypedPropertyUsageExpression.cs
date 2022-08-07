using Common;

namespace AST.Trees.Expressions.Typed
{
    public class TypedPropertyUsageExpression : TypedExpression
    {
        public TypedPropertyUsageExpression(
            DiagnosticInfo diag,
            TypeInfo objectTypeInfo,
            TypeInfo typeInfo,
            string variableName,
            string propertyName,
            int propertyIndex,
            UntypedScopeContext context) : base(diag, typeInfo, context)
        {
            VariableName = variableName;
            PropertyName = propertyName;
            PropertyIndex = propertyIndex;
            ObjectTypeInfo = objectTypeInfo;
        }

        public string VariableName { get; }

        public string PropertyName { get; }

        public int PropertyIndex { get; }

        public TypeInfo ObjectTypeInfo { get; }

        public override bool IsConstant()
        {
            return false;
        }

        public override string ToString()
        {
            return $"Typed Property Use: {VariableName}.{PropertyName}";
        }
    }
}
