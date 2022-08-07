using Common;
using AST.Trees;
using AST.Passes.Miscs;
using AST.Passes.Results;
using System.Collections.Generic;
using AST.Trees.Declarations.Untyped;

namespace AST.Passes
{
    public class TypeDiscoveryPass : IPass
    {
        public const string PassName = "TypeDiscoveryPass";

        public string Name { get; set; } = PassName;

        private readonly List<TypeInfo> KnownTypes = new();

        public List<FunctionInfo> KnownFunctions { get; set; } = new();

        public PassesExchangePoint Exchange { get; set; }

        public PassResult Walk(Node root, PassesExchangePoint exchange)
        {
            Exchange = exchange;
            DiscoverTypes(root);
            return new TypeDiscoveryPassResult(PassName, KnownTypes, KnownFunctions);
        }

        private void DiscoverTypes(Node root)
        {
            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current is UntypedFunctionNode ufn)
                {
                    var functionInfo = new FunctionInfo
                    (
                        ufn.Name,
                        ufn.DesiredType,
                        ufn.Arguments,
                        ufn.ScopeContext
                    );

                    KnownFunctions.Add(functionInfo);
                }
                else if (current is UntypedContainerNode ucn)
                {
                    var typeInfo = new InitializableTypeInfo(ucn.Name);

                    foreach (var entry in ucn.Fields)
                    {
                        typeInfo.InitializationTypesOrdered.Add((entry.Name, entry.DesiredType));
                    }

                    KnownTypes.Add(typeInfo);
                }

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
    }
}
