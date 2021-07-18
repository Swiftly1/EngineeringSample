using Common;
using System.Collections.Generic;

namespace AST.Passes.Results
{
    public class TypeCheckerPassResult : PassResult
    {
        public TypeCheckerPassResult(string name, List<Message> errors) : base(name)
        {
            Errors.AddMessages(errors);
        }
    }
}
