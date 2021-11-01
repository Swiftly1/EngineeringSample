namespace Common
{
    public interface IMessagesPrinter
    {
        // leaky leaky NewLine & Color

        public void PrintInformation(string s, int tabDepth = 0);
        public void PrintInformationNewLine(string s, int tabDepth = 0);

        public void PrintWarning(string s, int tabDepth = 0);
        public void PrintWarningNewLine(string s, int tabDepth = 0);

        public void PrintError(string s, int tabDepth = 0);
        public void PrintErrorNewLine(string s, int tabDepth = 0);

        public void PrintColorNewLine(string s, int tabDepth = 0);
        public void PrintColor(string s, int tabDepth = 0);
    }
}
