namespace Text2Abstraction
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
            return $"line number '{LineNumber}' at position '{Position}' around character '{Current}'.";
        }
    }
}
