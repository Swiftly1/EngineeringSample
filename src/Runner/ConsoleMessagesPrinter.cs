using System;
using System.Text;
using Common;

namespace Runner
{
    public class ConsoleMessagesPrinter : IMessagesPrinter
    {
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

        public void PrintInformationNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.White);
        }

        public void PrintWarningNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.Yellow);
        }

        public void PrintErrorNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.Red);
        }

        public void PrintFancyNewLine(string s)
        {
            PrintColorNewLine(s, ConsoleColor.Magenta);
        }

        public void PrintInformation(string s)
        {
            PrintColor(s, ConsoleColor.White);
        }

        public void PrintWarning(string s)
        {
            PrintColor(s, ConsoleColor.Yellow);
        }

        public void PrintError(string s)
        {
            PrintColor(s, ConsoleColor.Red);
        }

        public void PrintFancy(string s)
        {
            PrintColor(s, ConsoleColor.Magenta);
        }
    }
}
