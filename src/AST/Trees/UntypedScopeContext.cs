using AST.Trees.Miscs;
using System;
using System.Collections.Generic;

#nullable enable
namespace AST.Trees
{
    public class UntypedScopeContext
    {
        public UntypedScopeContext(string name_space)
        {
            Name = name_space;
        }

        public UntypedScopeContext(UntypedScopeContext parent_context, string name)
        {
            Parent = parent_context;
            Name = name;
        }

        public UntypedScopeContext()
        {

        }

        // TODO: Maybe ScopeContext should be base class for more specific kinds of contexts?
        public string? Name { get; set; }

        public UntypedScopeContext? Parent { get; set; }

        private List<BasicVariableDescription> DeclaredVariables { get; set; } = new();

        public void DeclareVariable(BasicVariableDescription desc)
        {
            if (desc is null)
                throw new ArgumentException(nameof(desc));

            DeclaredVariables.Add(desc);
        }

        public void DeclareVariables(List<BasicVariableDescription> descList)
        {
            foreach (var item in descList)
                DeclareVariable(item);
        }

        public List<BasicVariableDescription> DeclaredVariablesList
        {
            get
            {
                var all = new List<BasicVariableDescription>();

                all.AddRange(DeclaredVariables);

                var current = Parent;

                while (current is not null)
                {
                    all.AddRange(current.DeclaredVariables);
                    current = Parent!.Parent;
                }

                return all;
            }
        }
    }
}