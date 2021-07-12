using Common;
using AST.Trees;
using AST.Passes.Results;
using System.Collections.Generic;

namespace AST.Passes
{
    public class TypeDiscoveryPass : IPass
    {
        public const string PassName = "TypeDiscoveryPass";

        public string Name { get; set; } = PassName;

        private List<TypeInfo> KnownTypes = new();

        public PassesExchangePoint Exchange { get; set; }

        public PassResult Walk(Node root, PassesExchangePoint exchange)
        {
            Exchange = exchange;
            DiscoverTypes(root);
            return new TypeDiscoveryPassResult(PassName, KnownTypes);
        }

        private void DiscoverTypes(Node root)
        {

        }
    }
}
