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
            List<ContainerField> fields
        ) : base(diag, context)
        {
            Name = name;
            AccessibilityModifier = accessMod;
            Fields = fields;
        }

        public string Name { get; }

        public LexKeyword AccessibilityModifier { get; set; }

        public List<ContainerField> Fields { get; set; } = new List<ContainerField>();

        public override string ToString()
        {
            return $"Container: '{Name}'";
        }
    }
}
