using AST.Trees.Miscs;
using System.Collections.Generic;

namespace AST.Trees
{
    public class ScopeContext
    {
        public ScopeContext Parent { get; set; }

        public List<VariableDeclarationInfo> DeclaredVariables { get; set; } = new();

        public List<VariableDeclarationInfo> DeclaredVariablesList()
        {
            var all = new List<VariableDeclarationInfo>();

            all.AddRange(DeclaredVariables);

            var current = Parent;
            while (current is not null)
            {
                all.AddRange(current.DeclaredVariables);
                current = Parent.Parent;
            }

            return all;
        }
    }
}