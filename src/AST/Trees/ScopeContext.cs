using AST.Trees.Miscs;
using System.Collections.Generic;

#nullable enable
namespace AST.Trees
{
    public class ScopeContext
    {
        // TODO: Maybe ScopeContext should be base class for more specific kinds of contexts?
        public string? Namespace { get; set; }

        public ScopeContext(string name_space)
        {
            Namespace = name_space;
        }

        public ScopeContext(ScopeContext parent_context)
        {
            Parent = parent_context;
        }

        public ScopeContext()
        {

        }

        public ScopeContext? Parent { get; set; }

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