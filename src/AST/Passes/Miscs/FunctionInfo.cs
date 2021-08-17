using AST.Trees;
using AST.Trees.Miscs;
using System.Collections.Generic;

namespace AST.Passes.Miscs
{
    public class FunctionInfo
    {
        public FunctionInfo(string name, string desiredType, List<Argument> arguments, ScopeContext scopeContext)
        {
            Name = name;
            DesiredType = desiredType;
            Arguments = arguments;
            ScopeContext = scopeContext;
        }

        public string Name { get; }

        public string DesiredType { get; }

        public List<Argument> Arguments { get; } = new List<Argument>();

        public ScopeContext ScopeContext { get; }
    }
}
