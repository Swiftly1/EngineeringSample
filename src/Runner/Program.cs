using System;
using Text2Abstraction;

namespace Runner
{
    static class Program
    {
        static void Main()
        {
            var test = new TextTransformer("abc test a123");
            test.Walk();
        }
    }
}
