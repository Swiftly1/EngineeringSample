namespace Common
{
    public class DiagnosticInfo
    {
        public int LineNumber { get; }

        public int Position { get; }

        public char Current { get; }

        public DiagnosticInfo(int lineNumber, int position, char current)
        {
            LineNumber = lineNumber;
            Position = position;
            Current = current;
        }

        public override string ToString()
        {
            return UseTemplate(LineNumber, Position, Current);
        }

        public const string DiagnosticTemplate = "line number '{0}' at position '{1}' at character '{2}'.";

        public static string UseTemplate(params object[] args) => string.Format(DiagnosticTemplate, args);
    }
}
