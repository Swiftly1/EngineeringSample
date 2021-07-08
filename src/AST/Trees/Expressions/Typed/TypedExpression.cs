using Common;

namespace AST.Trees.Expressions.Typed
{
    public abstract class TypedExpression : Expression
    {
        protected TypedExpression(DiagnosticInfo diag, TypeInfo typeInfo) : base(diag)
        {
            TypeInfo = typeInfo;
        }

        public TypeInfo TypeInfo { get; set; }

        public abstract override string ToString();
    }
}
