using Common;
using AST.Trees.Miscs;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Declarations.Untyped
{
    public class UntypedContainerNode : ScopeableNode
    {
        public UntypedContainerNode
        (
            DiagnosticInfo diag,
            string name,
            LexKeyword accessMod,
            UntypedScopeContext context,
            List<ContainerFieldNode> fields
        ) : base(diag, context)
        {
            Name = name;
            AccessibilityModifier = accessMod;
            Fields = fields;
            this.AddChildren(Fields);
        }

        public string Name { get; }

        public LexKeyword AccessibilityModifier { get; set; }

        public List<ContainerFieldNode> Fields { get; set; } = new List<ContainerFieldNode>();

        public override string ToString()
        {
            return $"Untyped Container: '{Name}'";
        }
    }
}
