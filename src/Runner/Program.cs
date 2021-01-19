using System;
using Text2Abstraction;

namespace Runner
{
    static class Program
    {
        static void Main()
        {
            var test = new TextTransformer("12 35.5").Walk();
        }
    }
}
