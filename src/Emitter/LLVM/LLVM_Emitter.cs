using Common;
using System;
using AST.Trees;

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
            else if (node is FunctionNode fn)
            {
                _printer.PrintColor($"define dso_local i32 @{fn.Name}");
            }
            else
            {
                // throw new Exception($"Unsupported Node kind by LLVM IR Emitter");
            }

            EmitSubNodes(node);
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
