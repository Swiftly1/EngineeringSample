namespace Common
{
    public interface IMessagesPrinter
    {
        public void PrintInformationNewLine(string s);

        public void PrintWarningNewLine(string s);

        public void PrintErrorNewLine(string s);

        public void PrintFancyNewLine(string s);

        public void PrintInformation(string s);

        public void PrintWarning(string s);

        public void PrintError(string s);

        public void PrintFancy(string s);
    }
}
