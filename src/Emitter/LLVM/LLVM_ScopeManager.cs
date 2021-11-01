using System.Collections.Generic;

namespace Emitter.LLVM
{
    public class LLVM_ScopeManager
    {
        public Stack<LLVM_ScopeInfo> Scopes { get; set; } = new();

        public LLVM_ScopeInfo GetLastScope() => Scopes.Peek();

        public LLVM_ScopeInfo AddScope()
        {
            var scope = new LLVM_ScopeInfo();

            Scopes.Push(scope);

            return scope;
        }
    }
}
