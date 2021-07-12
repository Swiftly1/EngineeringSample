using System;
using Common;
using AST.Trees;
using System.Linq;
using AST.Passes.Results;
using System.Collections.Generic;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;
using AST.Trees.Statements.Untyped;
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
            var queue = new Queue<(Node Child, Node Parent)>();

            // damn
            // it's pair of children and its parent
            // because we need parent to change reference from "erased" node to new node

            queue.Enqueue((root, null));

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                var result = TryBoundWholeNode(current.Child);

                if (result.OverwrittenNode is not null)
                {
                    ReplaceNode(current.Parent, current.Child, result.OverwrittenNode);
                }

                if (!result.SkipChildren)
                {
                    foreach (var child in current.Child.Children)
                    {
                        queue.Enqueue((child, current.Child));
                    }
                }
            }
        }

        private (bool SkipChildren, Node OverwrittenNode) TryBoundWholeNode(Node current)
        {
            if (current is UntypedVariableDeclarationStatement vds)
            {
                var type = vds.DesiredType;

                var result = FindTypeByName(type);

                if (!result.Found)
                {
                    Errors.AddError($"Type {type} is not found.", current.Diagnostics);
                    return (true, null);
                }

                var exprType = GenerateBoundedTreeAndGetType(vds.Expression);

                // TODO: Support inheritance
                if (result.TypeInfo != exprType.TypeInfo)
                {
                    Errors.AddError($"Cannot assign type {exprType.TypeInfo.Name} to {result.TypeInfo.Name}.", current.Diagnostics);
                    return (true, null);
                }

                var newNode = new TypedVariableDeclarationStatement(vds.VariableName, exprType.NewNode, exprType.TypeInfo, vds.Diagnostics);

                return (true, newNode);
            }

            return (false, null);
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

        private void ReplaceNode(Node parent, Node specific_child, Node overwrittenNode)
        {
            if (parent is null)
                throw new Exception("Cannot replace child Node of null parent.");

            var newChildrenList = new List<Node>();

            var swapped = false;

            foreach (var child in parent.Children)
            {
                if (child.Id == specific_child.Id)
                {
                    newChildrenList.Add(overwrittenNode);
                    swapped = true;
                }
                else
                {
                    newChildrenList.Add(child);
                }
            }

            if (!swapped)
                throw new Exception("Catastrophic Failure. Unable to find Node to replace in parent's collection. Please report.");

            parent.Children = newChildrenList;
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
