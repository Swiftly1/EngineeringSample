using AST.Trees;
using AST.Passes.Results;

namespace AST.Passes
{
    public interface IPass
    {
        public PassResult Walk(Node root, PassesExchangePoint exchange);
    }
}