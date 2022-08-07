using Common;

namespace AST.Trees.Miscs
{
    public class ContainerFieldNode : Node
    {
        public ContainerFieldNode(DiagnosticInfo diag) : base(diag)
        {
        }

        public ContainerFieldNode(string name, string desiredType, DiagnosticInfo nameDiagnostic, DiagnosticInfo typeDiagnostic) : base(nameDiagnostic)
        {
            Name = name;
            DesiredType = desiredType;
            NameDiagnostic = nameDiagnostic;
            TypeDiagnostic = typeDiagnostic;
        }

        public string Name { get; }

        public string DesiredType { get; }

        public DiagnosticInfo NameDiagnostic { get; }

        public DiagnosticInfo TypeDiagnostic { get; }

        public override string ToString()
        {
            return $"{Name} of type {DesiredType}";
        }
    }
}
