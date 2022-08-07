using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class UntypedPropertyUsageExpression : UntypedExpression
    {
        public UntypedPropertyUsageExpression(DiagnosticInfo diag, string variableName, string propertyName, UntypedScopeContext context) : base(diag, context)
        {
            VariableName = variableName;
            PropertyName = propertyName;
        }

        public string VariableName { get; }

        public string PropertyName { get; }

        public override string ToString()
        {
            return $"Untyped Property Use: {VariableName}.{PropertyName}";
        }
    }
}
