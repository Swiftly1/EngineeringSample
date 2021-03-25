using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST.Miscs;
using Common;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace AST.Builders
{
    public partial class ASTBuilder : Movable<LexElement>
    {
        private readonly ErrorHandler _errors = new ErrorHandler();

        public ASTBuilder(List<LexElement> collection) : base(collection)
        {
        }

        public ASTBuilder Build()
        {
            var splitted = SplitByNamespace();

            /// It's way easier to debug non-Parallel code
            #if DEBUG
                foreach (var item in splitted)
                {
                    var unit = new NamespaceBuilder(item).Build();
                }
            #else
                Parallel.ForEach(splitted, (GroupedLexicalElements item) =>
                {
                    var unit = new NamespaceBuilder(item).Build();
                });
            #endif

            return this;
        }

        private List<GroupedLexicalElements> SplitByNamespace()
        {
            var list = new List<GroupedLexicalElements>();

            GroupedLexicalElements current_group = null;

            do
            {
                if (current_group is null && _Current.Kind != LexingElement.Namespace)
                {
                    _errors.AddError($"Every element must be within some namespace.", _Current.Diagnostics);
                }
                else if (_Current.Kind is LexingElement.Namespace)
                {
                    if (current_group != null)
                    {
                        list.Add(current_group);
                    }

                    current_group = new GroupedLexicalElements(_Current.Diagnostics);
                }
                else
                {
                    current_group.AddElement(_Current);
                }

                if (IsLast())
                {
                    list.Add(current_group);
                }
            } while (MoveNext());

            foreach (var group in list)
            {
                if (group.Elements.Any() && group.Elements.First() is LexKeyword first)
                {
                    group.NamespaceName = first.Value;
                }
                else
                {
                    _errors.AddError("Namespace must be named. Example: 'namespace Person'", group.Diagnostics);
                }
            }

            return list;
        }
    }
}
