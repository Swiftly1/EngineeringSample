using AST.Trees;
using System.Collections.Generic;
using System.Linq;

namespace AST.Passes
{
    public class PassManager
    {
        private PassManager() { }

        public List<IPass> Passes { get; set; } = new();

        public PassesExchangePoint Exchange { get; set; } = new();

        public void WalkAll(RootNode root)
        {
            foreach (var pass in Passes)
            {
                var result = pass.Walk(root, Exchange);
                Exchange.PassResults.Add(result.PassName, result);
            }
        }

        public static PassManager GetDefaultPassManager()
        {
            var pm = new PassManager();
            pm.Passes.Add(new TypeDiscoveryPass());
            pm.Passes.Add(new TypeCheckerPass());
            pm.Passes.Add(new ShortIdGeneratorPass());
            return pm;
        }

        public static PassManager GetEmptyPassManager() => new PassManager();
    }
}