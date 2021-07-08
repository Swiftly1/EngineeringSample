using Common;
using AST.Trees;

namespace Emitter
{
    public abstract class BaseEmitter
    {
        protected readonly IMessagesPrinter _printer;

        protected BaseEmitter(IMessagesPrinter printer)
        {
            _printer = printer;
        }

        public abstract Result Emit(Node node);
    }
}
