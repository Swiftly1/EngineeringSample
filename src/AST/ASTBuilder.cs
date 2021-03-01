using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace AST
{
    public class ASTBuilder : IMovable<LexElement>
    {
        private ErrorHandler _errors = new ErrorHandler();

        public ASTBuilder(List<LexElement> collection) : base(collection)
        {
        }

        public ASTBuilder Build()
        {
            var splitted = SplitByNamespace();
            return this;
        }

        private List<GroupedLexicalElements> SplitByNamespace()
        {
            var list = new List<GroupedLexicalElements>();

            GroupedLexicalElements current = null;

            for (int i = 0; i < _Collection.Count; i++)
            {
                if (current is null && _Collection[i].Kind != LexingElement.Namespace)
                {
                    _errors.AddError($"Every element must be within some namespace.", _Collection[i].Diagnostics);
                }
                else if (_Collection[i].Kind is LexingElement.Namespace)
                {
                    if (current != null)
                    {
                        list.Add(current);
                    }

                    current = new GroupedLexicalElements(_Collection[i].Diagnostics);
                }
                else
                {
                    current.AddElement(_Collection[i]);
                }

                if (i == _Collection.Count - 1)
                    list.Add(current);
            }

            foreach (var group in list)
            {
                if (group.Elements.Any() && group.Elements.First() is LexKeyword first)
                {
                    group.Namespace = first.Value;
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
