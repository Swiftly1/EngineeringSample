using Common;

namespace AST.Trees.Miscs
{
    public class ContainerField
    {
        public ContainerField(string name, string desiredType, DiagnosticInfo nameDiagnostic, DiagnosticInfo typeDiagnostic)
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
