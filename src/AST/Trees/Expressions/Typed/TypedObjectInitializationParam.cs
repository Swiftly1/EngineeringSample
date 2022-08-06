using AST.Trees.Expressions.Typed;
using Common;

namespace AST.Trees.Expressions.Typed
{
    public class TypedObjectInitializationParam : Node
    {
        public TypedObjectInitializationParam(DiagnosticInfo diag) : base(diag)
        {
        }

        public TypedObjectInitializationParam(
            string name,
            TypedExpression expression,
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

        public TypedExpression Expression { get; }

        public DiagnosticInfo NameDiagnostic { get; }

        public int Index { get; }

        public bool IsLast { get; set; }

        public override string ToString()
        {
            return $"{Name} with value: {Expression}";
        }
    }
}
