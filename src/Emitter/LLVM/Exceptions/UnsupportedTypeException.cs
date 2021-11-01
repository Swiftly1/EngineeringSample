using System;

namespace Emitter.LLVM.Exceptions
{
    public class UnsupportedTypeException : Exception
    {
        public UnsupportedTypeException() : base()
        {
        }

        public UnsupportedTypeException(string message) : base(message)
        {
        }

        public UnsupportedTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
