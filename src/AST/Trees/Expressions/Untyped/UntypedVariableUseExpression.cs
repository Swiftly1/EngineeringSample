using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class UntypedVariableUseExpression : UntypedExpression
    {
        public UntypedVariableUseExpression(DiagnosticInfo diag, string variableName, ScopeContext context) : base(diag, context)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }

        public override string ToString()
        {
            return $"Untyped Variable Use: {VariableName}";
        }
    }
}
