using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AST.Miscs;
using AST.Trees;
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

        public Result<RootNode> Build()
        {
            var root = new RootNode(new DiagnosticInfo(0, 0, ' '));

            var splitted = SplitByNamespace();

            /// It's way easier to debug non-Parallel code
            #if DEBUG
                foreach (var item in splitted)
                {
                    var unit = new NamespaceBuilder(item).Build();
                    root.AddChild(unit);
                }
            #else
                Parallel.ForEach(splitted, (GroupedLexicalElements item) =>
                {
                    var unit = new NamespaceBuilder(item).Build();
                });
            #endif

            if (_errors.DumpErrors().Any())
                return new Result<RootNode>(_errors.DumpErrors().ToList());

            return new Result<RootNode>(root);
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
                if (group.Elements.Any() && group.Elements.First() is LexWord first)
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
