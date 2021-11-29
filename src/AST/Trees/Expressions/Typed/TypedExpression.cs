using Common;

namespace AST.Trees.Expressions.Typed
{
    public abstract class TypedExpression : Expression
    {
        protected TypedExpression(DiagnosticInfo diag, TypeInfo typeInfo, UntypedScopeContext context) : base(diag, context)
        {
            TypeInfo = typeInfo;
        }

        public TypeInfo TypeInfo { get; set; }

        public abstract bool IsConstant();

        public abstract override string ToString();
    }
}
