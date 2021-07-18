﻿using Common;
using AST.Miscs;
using AST.Trees;
using AST.Passes;
using System.Linq;
using Common.Lexing;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Builders
{
    public partial class ASTBuilder : Movable<LexElement>
    {
        private readonly ErrorHandler _errors = new();

        public readonly PassManager Passes = PassManager.GetDefaultPassManager();

        public ASTBuilder(List<LexElement> collection) : base(collection)
        {
        }

        public ResultDiag<RootNode> Build()
        {
            var root = new RootNode(new DiagnosticInfo(0, 0, ' '));

            var splitted = SplitByNamespace();

            foreach (var item in splitted)
            {
                var unit = new NamespaceBuilder(item).TryBuild();

                if (!unit.Success)
                {
                    return unit.ToFailedResult<RootNode>();
                }

                root.AddChild(unit.Data);
            }

            if (_errors.DumpErrors().Any())
            {
                return new ResultDiag<RootNode>(_errors.DumpErrors().ToList());
            }
            else
            {
                var passResult = Passes.WalkAll(root);

                if (!passResult.Success)
                {
                    _errors.AddMessages(passResult.Messages);
                    return new ResultDiag<RootNode>(_errors.DumpErrors().ToList());
                }
            }

            return new ResultDiag<RootNode>(root);
        }

        private List<GroupedLexicalElements> SplitByNamespace()
        {
            var list = new List<GroupedLexicalElements>();

            GroupedLexicalElements current_group = null;

            do
            {
                if (_Current.Kind is LexingElement.NewLine)
                {
                    // Including NewLines makes too much trouble
                    // Benefits of doing so aren't known at the time
                    // if (current_group != null)
                    // {
                    //     current_group.AddElement(_Current);
                    // }
                }
                else if (current_group is null && _Current.Kind != LexingElement.Namespace)
                {
                    _errors.AddError("Every element must be within some namespace.", _Current.Diagnostics);
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
