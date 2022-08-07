using Common;
using System.Collections.Generic;

namespace AST.Trees.Expressions.Typed
{
    public class TypedNewExpression : TypedExpression
    {
        public TypedNewExpression(
            DiagnosticInfo diag,
            TypeInfo typeInfo,
            List<TypedObjectInitializationParam> initializationList,
            UntypedScopeContext context) : base(diag, typeInfo, context)
        {
            InitializationList = initializationList;
            AddChildren(initializationList);
        }

        public List<TypedObjectInitializationParam> InitializationList { get; } = new List<TypedObjectInitializationParam>();

        public override bool IsConstant()
        {
            return false;
        }

        public override string ToString()
        {
            return $"Typed New Call: {this.TypeInfo.Name}";
        }
    }
}
