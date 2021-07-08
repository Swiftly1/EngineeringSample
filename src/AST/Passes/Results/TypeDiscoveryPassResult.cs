using Common;
using System.Collections.Generic;

namespace AST.Passes.Results
{
    public class TypeDiscoveryPassResult : PassResult
    {
        public TypeDiscoveryPassResult(string name, List<TypeInfo> knownTypes) : base(name)
        {
            KnownTypes = knownTypes;
        }

        public List<TypeInfo> KnownTypes { get; set; } = new();
    }
}
