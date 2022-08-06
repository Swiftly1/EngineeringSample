using Common;

namespace AST.Trees.Expressions.Untyped
{
    public class UntypedObjectInitializationParam : Node
    {
        public UntypedObjectInitializationParam(DiagnosticInfo diag) : base(diag)
        {
        }

        public UntypedObjectInitializationParam(
            string name,
            UntypedExpression expression,
            DiagnosticInfo nameDiagnostic,
            int index,
            bool isLast) : base(nameDiagnostic)
        {
            Name = name;
            Expression = expression;
            NameDiagnostic = nameDiagnostic;
            Index = index;
            IsLast = isLast;
        }

        public string Name { get; }

        public UntypedExpression Expression { get; }

        public DiagnosticInfo NameDiagnostic { get; }

        public int Index { get; }

        public bool IsLast { get; set; }

        public override string ToString()
        {
            return $"{Name} with value: {Expression}";
        }
    }
}
