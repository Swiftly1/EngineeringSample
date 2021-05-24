using System;
using System.Collections.Generic;
using AST.Trees;

namespace AST.Passes
{
    public class PassManager
    {
        private PassManager() { }

        public List<IPass> Passes { get; set; } = new List<IPass>();

        public void WalkAll(RootNode root)
        {
            foreach (var pass in Passes)
            {
                pass.Walk(root);
            }
        }

        public static PassManager GetDefaultPassManager()
        {
            var pm = new PassManager();
            pm.Passes.Add(new ShortIdGeneratorPass());
            return pm;
        }

        public static PassManager GetEmptyPassManager() => new PassManager();
    }
}