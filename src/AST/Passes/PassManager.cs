using System;
using Common;
using AST.Trees;
using System.Linq;
using System.Collections.Generic;

namespace AST.Passes
{
    public class PassManager
    {
        private PassManager() { }

        public List<IPass> Passes { get; set; } = new();

        public PassesExchangePoint Exchange { get; set; } = new();

        public ResultDiag<bool> WalkAll(RootNode root)
        {
            // Ensure Unique Pass Names

            var duplicated_names = Passes
                                    .Select(x => x.Name)
                                    .GroupBy(x => x)
                                    .Where(x => x.Count() > 1)
                                    .Select(x => x.Key)
                                    .ToList();

            if (duplicated_names.Any())
            {
                var join = string.Join(',', duplicated_names);
                throw new Exception($"Names of the Passes aren't unique. Those are problematic: [{join}].");
            }

            foreach (var pass in Passes)
            {
                var result = pass.Walk(root, Exchange);
                Exchange.PassResults.Add(result.PassName, result);

                if (result.FullStop)
                {
                    break;
                }
            }

            var errors = Exchange.PassResults.SelectMany(x => x.Value.Errors.DumpErrors()).ToList();
            return errors.Any() ? new ResultDiag<bool>(errors) : new ResultDiag<bool>(true);
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