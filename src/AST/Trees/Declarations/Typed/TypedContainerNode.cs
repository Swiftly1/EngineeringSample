using Common;
using AST.Trees.Miscs;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Declarations.Typed
{
    public class TypedContainerNode : ScopeableNode
    {
        public TypedContainerNode
        (
            DiagnosticInfo diag,
            string name,
            LexKeyword accessMod,
            UntypedScopeContext context,
            List<TypedContainerFieldNode> fields
        ) : base(diag, context)
        {
            Name = name;
            AccessibilityModifier = accessMod;
            Fields = fields;
            this.AddChildren(Fields);
        }

        public string Name { get; }

        public LexKeyword AccessibilityModifier { get; set; }

        public List<TypedContainerFieldNode> Fields { get; set; } = new List<TypedContainerFieldNode>();

        public override string ToString()
        {
            return $"Typed Container: '{Name}'";
        }
    }
}
