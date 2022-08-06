using Common;

namespace AST.Trees.Miscs
{
    public class TypedContainerFieldNode : Node
    {
        public TypedContainerFieldNode(DiagnosticInfo diag) : base(diag)
        {
        }

        public TypedContainerFieldNode(
            string name,
            TypeInfo type,
            DiagnosticInfo nameDiagnostic,
            DiagnosticInfo typeDiagnostic,
            int index,
            bool isLast) : base(nameDiagnostic)
        {
            Name = name;
            TypeInfo = type;
            NameDiagnostic = nameDiagnostic;
            TypeDiagnostic = typeDiagnostic;
            Index = index;
            IsLast = isLast;
        }

        public string Name { get; }

        public TypeInfo TypeInfo { get; }

        public DiagnosticInfo NameDiagnostic { get; }

        public DiagnosticInfo TypeDiagnostic { get; }

        public int Index { get; }

        public bool IsLast { get; }

        public override string ToString()
        {
            return $"{Name} of type {TypeInfo.Name}";
        }
    }
}
