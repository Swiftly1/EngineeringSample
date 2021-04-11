using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Miscs.Matching
{
    public class EvaluationResult
    {
        public EvaluationResult(bool success, List<LexElement> items)
        {
            Success = success;
            Items = items;
        }

        public EvaluationResult(bool success, LexElement failedElement)
        {
            Success = success;
            Items = new List<LexElement>();
            FailedElement = failedElement;
        }

        public bool Success { get; set; }

        public List<LexElement> Items { get; set; }

        public LexElement FailedElement { get; set; }
    }
}
