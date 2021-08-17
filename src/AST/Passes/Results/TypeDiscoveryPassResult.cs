using Common;
using AST.Passes.Miscs;
using System.Collections.Generic;

namespace AST.Passes.Results
{
    public class TypeDiscoveryPassResult : PassResult
    {
        public TypeDiscoveryPassResult(string name, List<TypeInfo> knownTypes, List<FunctionInfo> knownFunctions) : base(name)
        {
            KnownTypes = knownTypes;
            KnownFunctions = knownFunctions;
        }

        public List<TypeInfo> KnownTypes { get; set; } = new();

        public List<FunctionInfo> KnownFunctions { get; set; } = new();
    }
}
