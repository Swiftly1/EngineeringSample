using Common;
using System;
using AST.Trees;
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
                _printer.PrintColor($"define dso_local i32 @{fn.Name}({args})");
            }
            else
            {
                // throw new Exception($"Unsupported Node kind by LLVM IR Emitter");
            }

            EmitSubNodes(node);
        }

        private string FunctionArgsToLLVM(TypedFunctionNode fn)
        {
            throw new NotImplementedException();
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
