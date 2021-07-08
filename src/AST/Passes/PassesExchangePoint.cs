using AST.Passes.Results;
using System.Collections.Generic;

namespace AST.Passes
{
    public class PassesExchangePoint
    {
        public Dictionary<string, PassResult> PassResults { get; set; } = new();
    }
}
