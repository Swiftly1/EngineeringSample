using Common;
using System;

namespace Emitter.LLVM
{
    public static class ExtensionMethods
    {
        public static string ToLLVMType(this TypeInfo t)
        {
            if (t == TypeFacts.GetBoolean())
            {
                return "i1";
            }
            else if (t == TypeFacts.GetInt32())
            {
                return "i32";
            }
            else if (t == TypeFacts.GetInt64())
            {
                return "i64";
            }
            else if (t == TypeFacts.GetVoid())
            {
                return "void";
            }
            else if (t == TypeFacts.GetDouble())
            {
                return "double";
            }
            else
            {
                throw new NotImplementedException($"Unknwon type {t}");
            }
        }
    }
}
