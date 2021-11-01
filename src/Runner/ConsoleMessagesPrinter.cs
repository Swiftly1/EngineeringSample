using System;
using Common;

namespace Runner
{
    public class ConsoleMessagesPrinter : IMessagesPrinter
    {
        public ConsoleColor Color { get; set; } = ConsoleColor.Magenta;

        private void PrintColorNewLine(string s, ConsoleColor color, int tabDepth)
        {
            Console.ForegroundColor = color;
            var tab = new string('\t', tabDepth);
            Console.WriteLine(tab + s);
            Console.ResetColor();
        }

        private void PrintColor(string s, ConsoleColor color, int tabDepth)
        {
            Console.ForegroundColor = color;
            var tab = new string('\t', tabDepth);
            Console.Write(tab + s);
            Console.ResetColor();
        }

        public void PrintInformation(string s, int tabDepth = 0)
        {
            PrintColor(s, ConsoleColor.White, tabDepth);
        }

        public void PrintInformationNewLine(string s, int tabDepth = 0)
        {
            PrintColorNewLine(s, ConsoleColor.White, tabDepth);
        }

        public void PrintWarning(string s, int tabDepth = 0)
        {
            PrintColor(s, ConsoleColor.Yellow, tabDepth);
        }

        public void PrintWarningNewLine(string s, int tabDepth = 0)
        {
            PrintColorNewLine(s, ConsoleColor.Yellow, tabDepth);
        }

        public void PrintError(string s, int tabDepth = 0)
        {
            PrintColor(s, ConsoleColor.Red, tabDepth);
        }
        public void PrintErrorNewLine(string s, int tabDepth = 0)
        {
            PrintColorNewLine(s, ConsoleColor.Red, tabDepth);
        }

        public void PrintColor(string s, int tabDepth = 0)
        {
            PrintColor(s, this.Color, tabDepth);
        }

        public void PrintColorNewLine(string s, int tabDepth = 0)
        {
            PrintColorNewLine(s, this.Color, tabDepth);
        }
    }
}
