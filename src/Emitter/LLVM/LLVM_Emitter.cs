using Common;
using System;
using AST.Trees;
using System.Text;
using AST.Trees.Declarations.Typed;

namespace Emitter.LLVM
{
    public class LLVM_Emitter : BaseEmitter
    {
        public LLVM_Emitter(IMessagesPrinter printer) : base(printer)
        {
        }

        public override Result Emit(Node node)
        {
            try
            {
                InternalEmit(node);
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }

            return new Result();
        }

        private void InternalEmit(Node node)
        {
            if (node is RootNode rn)
            {

            }
            else if (node is TypedFunctionNode fn)
            {
                var args = FunctionArgsToLLVM(fn);
                _printer.PrintColorNewLine($"define dso_local {fn.Type.ToLLVMType()} @{fn.Name}({args})");
                _printer.PrintColorNewLine("");
            }
            else
            {
                // throw new Exception($"Unsupported Node kind by LLVM IR Emitter");
            }

            EmitSubNodes(node);
        }

        private string FunctionArgsToLLVM(TypedFunctionNode fn)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < fn.Arguments.Count; i++)
            {
                var current = fn.Arguments[i];
                sb.Append($"{current.Type.ToLLVMType()} %{i}");

                if (i != fn.Arguments.Count - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }

        private void EmitSubNodes(Node node)
        {
            foreach (var sub_node in node.Children)
            {
                InternalEmit(sub_node);
            }
        }
    }
}
