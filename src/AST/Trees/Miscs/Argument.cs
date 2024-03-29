﻿using Common;

namespace AST.Trees.Miscs
{
    public class Argument
    {
        public Argument(string typeName, string name, DiagnosticInfo diagnostic)
        {
            TypeName = typeName;
            Name = name;
            Diagnostic = diagnostic;
        }

        public string TypeName { get; set; }

        public string Name { get; set; }

        public DiagnosticInfo Diagnostic { get; set; }
    }
}
