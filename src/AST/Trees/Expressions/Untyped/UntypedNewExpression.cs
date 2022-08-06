using Common;
using System.Collections.Generic;

namespace AST.Trees.Expressions.Untyped
{
    public class UntypedNewExpression : UntypedExpression
    {
        public UntypedNewExpression(
            DiagnosticInfo diag,
            string typeName,
            List<UntypedObjectInitializationParam> initializationList,
            UntypedScopeContext context) : base(diag, context)
        {
            DesiredType = typeName;
            InitializationList = initializationList;
        }

        public List<UntypedObjectInitializationParam> InitializationList { get; } = new List<UntypedObjectInitializationParam>();

        public string DesiredType { get; }

        public override string ToString()
        {
            return $"Untyped New Call: {DesiredType}";
        }
    }
}
