namespace Common
{
    public interface IMessagesPrinter
    {
        // just leaky NewLine & Color abstraction?

        public void PrintInformation(string s);
        public void PrintInformationNewLine(string s);

        public void PrintWarning(string s);
        public void PrintWarningNewLine(string s);

        public void PrintError(string s);
        public void PrintErrorNewLine(string s);

        public void PrintColorNewLine(string s);
        public void PrintColor(string s);
    }
}
