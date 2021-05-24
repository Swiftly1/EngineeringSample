using AST.Trees;

namespace AST.Passes
{
    public interface IPass
    {
        public void Walk(Node root);
    }
}