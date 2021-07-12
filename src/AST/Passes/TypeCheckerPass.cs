using System;
using Common;
using AST.Trees;
using System.Linq;
using AST.Passes.Results;
using AST.Trees.Statements;
using System.Collections.Generic;
using AST.Trees.Expressions.Typed;
using AST.Trees.Expressions.Untyped;

namespace AST.Passes
{
    public class TypeCheckerPass : IPass
    {
        public const string PassName = "TypeCheckerPass";

        public string Name { get; set; } = PassName;

        public PassesExchangePoint Exchange { get; set; }

        public List<TypeInfo> KnownTypes { get; set; } = new();

        public ErrorHandler Errors { get; set; } = new();

        public PassResult Walk(Node root, PassesExchangePoint exchange)
        {
            Exchange = exchange;
            KnownTypes = (Exchange.PassResults[TypeDiscoveryPass.PassName] as TypeDiscoveryPassResult).KnownTypes;
            CheckTree(root);
            return new EmptyPassResult(PassName);
        }

        private void CheckTree(Node root)
        {
            var queue = new Queue<Node>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var skip_children = TryBoundWholeNode(current);

                if (!skip_children)
                {
                    foreach (var child in current.Children)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        private bool TryBoundWholeNode(Node current)
        {
            if (current is VariableDeclarationStatement vds)
            {
                var type = vds.DesiredType;

                var result = FindTypeByName(type);

                if (!result.Found)
                {
                    Errors.AddError($"Type {type} is not found.", current.Diagnostics);
                    return true;
                }

                var exprType = GenerateBoundedTreeAndGetType(vds.Expression);

                // TODO: Support inheritance
                if (result.TypeInfo != exprType.TypeInfo)
                {
                    Errors.AddError($"Cannot assign type {exprType.TypeInfo.Name} to {result.TypeInfo.Name}.", current.Diagnostics);
                    return true;
                }

                vds.Children.RemoveAll(x => x.Id == vds.Expression.Id);
                vds.Expression = exprType.NewNode;
                vds.Children.Add(exprType.NewNode);

                return true;
            }

            return false;
        }

        private (bool Found, TypeInfo TypeInfo, TypedExpression NewNode) GenerateBoundedTreeAndGetType(Node expression)
        {
            if (expression is ConstantMathUntypedExpression cme)
            {
                var strValue = cme.Value.ToString();

                object value = null;
                TypeInfo type = null;

                if (int.TryParse(strValue, out var intValue))
                {
                    type = TypeFacts.GetInt32();
                    value = intValue;
                }

                if (type is null && double.TryParse(strValue, out var doubleValue))
                {
                    type = TypeFacts.GetDouble();
                    value = doubleValue;
                }

                if (type != null)
                {
                    var newExpr = new ConstantMathTypedExpression(expression.Diagnostics, value, type);
                    expression.CopyTo(newExpr);
                    expression = newExpr;

                    return (true, newExpr.TypeInfo, newExpr);
                }

                return (false, null, null);
            }
            else if (expression is ComplexUntypedExpression cue)
            {
                var leftResult = GenerateBoundedTreeAndGetType(cue.Left);
                var rightResult = GenerateBoundedTreeAndGetType(cue.Right);

                if (!leftResult.Found  || !rightResult.Found)
                {
                    return (false, null, null);
                }

                var found = OperatorFacts.TryMatch(leftResult.TypeInfo, rightResult.TypeInfo);

                if (found is null)
                    return (false, null, null);

                var newExpression = new ComplexTypedExpression
                (
                    leftResult.NewNode,
                    rightResult.NewNode,
                    cue.Operator,
                    found.ResultType,
                    cue.Diagnostics
                );

                cue.CopyTo(newExpression);

                return (true, found.ResultType, newExpression);
            }

            throw new NotImplementedException($"Node type {expression.GetType()} is not handled");
        }

        private (bool Found, TypeInfo TypeInfo) FindTypeByName(string desiredType)
        {
            if (TypeFacts.TypeName2TypeMapper.TryGetValue(desiredType, out var type))
            {
                return (true, type);
            }

            var foundType = KnownTypes.FirstOrDefault(x => x.Name == desiredType);

            return (foundType is not null, foundType);
        }
    }
}
