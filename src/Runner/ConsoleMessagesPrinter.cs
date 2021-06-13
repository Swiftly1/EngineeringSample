using System;
using Common;

namespace Runner
{
    public class ConsoleMessagesPrinter : IMessagesPrinter
    {
        public ConsoleColor Color { get; set; } = ConsoleColor.Magenta;

        private void PrintColorNewLine(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        private void PrintColor(string s, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(s);
            Console.ResetColor();
        }

        public void PrintInformation(string s)
        {
            PrintColor(s, ConsoleColor.White);
        }

        public void PrintInformationNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.White);
        }

        public void PrintWarning(string s)
        {
            PrintColor(s, ConsoleColor.Yellow);
        }

        public void PrintWarningNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.Yellow);
        }

        public void PrintError(string s)
        {
            PrintColor(s, ConsoleColor.Red);
        }
        public void PrintErrorNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.Red);
        }

        public void PrintColor(string s)
        {
            PrintColor(s, this.Color);
        }

        public void PrintColorNewLine(string s)
        {
            PrintColorNewLine(s, this.Color);
        }
    }
}
