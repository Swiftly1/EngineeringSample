using Common;

namespace AST.Trees.Miscs
{
    public class TypedArgument
    {
        public TypedArgument(TypeInfo type, string name, DiagnosticInfo diagnostic)
        {
            TypeInfo = type;
            Name = name;
            Diagnostic = diagnostic;
        }

        public TypeInfo TypeInfo { get; set; }

        public string Name { get; set; }

        public DiagnosticInfo Diagnostic { get; set; }
    }
}
