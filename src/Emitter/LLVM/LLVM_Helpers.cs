using AST.Trees.Declarations.Typed;
using System.Text;

namespace Emitter.LLVM
{
    public static class LLVM_Helpers
    {
        public static string FunctionArgsToLLVM(TypedFunctionNode fn)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < fn.Arguments.Count; i++)
            {
                var current = fn.Arguments[i];
                sb.Append($"{current.TypeInfo.ToLLVMType()} %{i}");

                if (i != fn.Arguments.Count - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }
    }
}
