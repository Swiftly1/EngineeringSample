namespace Text2Abstraction
{
    public class Settings
    {
        /// <summary>
        /// Indicates whether TextTransformer should attach LexingElement that represents NewLine
        /// TODO: Probably refactor/redesign needed.
        /// </summary>
        public bool NewLineAware { get; set; } = true;
    }
}