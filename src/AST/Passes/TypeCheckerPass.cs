using System;
using Common;
using AST.Trees;
using System.Linq;
using AST.Trees.Miscs;
using AST.Passes.Results;
using System.Collections.Generic;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;
using AST.Trees.Declarations.Typed;
using AST.Trees.Statements.Untyped;
using AST.Trees.Expressions.Untyped;
using AST.Trees.Declarations.Untyped;

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
            return new TypeCheckerPassResult(PassName, Errors.DumpErrors().ToList());
        }

        // TODO: Would be nice if error handling was done better here
        // e.g by using Result classes

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

                if (!exprType.Found)
                {
                    return (true, null);
                }

                // TODO: Support inheritance
                if (result.TypeInfo != exprType.TypeInfo)
                {
                    Errors.AddError($"Cannot assign type {exprType.TypeInfo.Name} to {result.TypeInfo.Name}.", current.Diagnostics);
                    return (true, null);
                }

                var newNode = new TypedVariableDeclarationStatement(vds.VariableName, exprType.NewNode, exprType.TypeInfo, vds.Diagnostics);

                return (true, newNode);
            }
            if (current is UntypedReturnStatement urs)
            {
                var bounded = GenerateBoundedTreeAndGetType(urs.ReturnExpression);

                if (!bounded.Found)
                {
                    return (true, null);
                }

                var newNode = new TypedReturnStatement(bounded.NewNode, urs.Diagnostics);

                return (true, newNode);
            }
            else if (current is UntypedFunctionNode ufn)
            {
                var typeResult = FindTypeByName(ufn.DesiredType);

                if (!typeResult.Found)
                {
                    Errors.AddError($"Type {ufn.DesiredType} is not found.", ufn.TypeDiagnostics);
                    return (true, null);
                }

                var argValidationFailed = false;
                var typedArgs = new List<TypedArgument>();

                foreach (var argType in ufn.Arguments)
                {
                    var argResult = FindTypeByName(argType.TypeName);

                    if (!argResult.Found)
                    {
                        Errors.AddError($"Type {argType.TypeName} is not found.", argType.Diagnostic);
                        // in order to get more error messages.
                        argValidationFailed = true;
                    }
                    else
                    {
                        typedArgs.Add(new TypedArgument(argResult.TypeInfo, argType.Name, argType.Diagnostic));
                    }
                }

                if (argValidationFailed)
                    return (true, null);

                var ensureResult = EnsureCorrectReturnTypeAtAllBranches(ufn, typeResult.TypeInfo);

                if (!ensureResult.Success)
                {
                    Errors.AddError($"Not all pathes in function at {ufn.Diagnostics} are " +
                        $"covered with return statement of type {typeResult.TypeInfo.Name}.", current.Diagnostics);

                    return (false, null);
                }

                var newNode = new TypedFunctionNode
                (
                    ufn.Diagnostics,
                    ufn.Name,
                    ufn.Body,
                    typedArgs,
                    typeResult.TypeInfo,
                    ufn.ScopeContext,
                    ufn.TypeDiagnostics,
                    ufn.AccessibilityModifierDiagnostics
                );

                return (false, newNode);
            }

            return (false, null);
        }

        private (bool Success, Node IncorrectNode) EnsureCorrectReturnTypeAtAllBranches(UntypedFunctionNode ufn, TypeInfo typeInfo)
        {
            // TODO: Ensure that all branches end with correct return statement type
            return (true, null);
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
                    var newExpr = new ConstantTypedExpression(expression.Diagnostics, value, type);
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

                if (!leftResult.Found || !rightResult.Found)
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
            else if (expression is ConstantUntypedStringExpression cuse)
            {
                var newExpression = new ConstantTypedExpression(cuse.Diagnostics, cuse.Value, TypeFacts.GetString());

                cuse.CopyTo(newExpression);

                return (true, newExpression.TypeInfo, newExpression);
            }
            else if (expression is UntypedFunctionCallExpression ufce)
            {
                var discoveryResult = Exchange.PassResults[TypeDiscoveryPass.PassName] as TypeDiscoveryPassResult;

                // TODO: It's temporary naive solution, we need to take into account different namespaces, classes and stuff.
                var foundFunction = discoveryResult.KnownFunctions.FirstOrDefault(x => x.Name == ufce.FunctionName);

                if (foundFunction is null)
                {
                    Errors.AddError($"Function with name '{ufce.FunctionName}' is not defined anywhere.", ufce.Diagnostics);
                    return (false, null, null);
                }

                if (foundFunction.Arguments.Count != ufce.CallArguments.Count)
                {
                    Errors.AddError($"Function call of '{ufce.FunctionName}' function has incorrect number of arguments.", ufce.Diagnostics);
                    return (false, null, null);
                }

                var typedCallArgs = new List<TypedExpression>();
                for (int i = 0; i < ufce.CallArguments.Count; i++)
                {
                    var current = ufce.CallArguments[i];
                    var typeResult = GenerateBoundedTreeAndGetType(current);

                    if (!typeResult.Found)
                    {
                        Errors.AddError($"Type for argument of Expression at {i} index cannot be resolved.", current.Diagnostics);
                        return (false, null, null);
                    }

                    var argType = FindTypeByName(foundFunction.Arguments[i].TypeName);
                    if (!argType.Found || (argType.TypeInfo != typeResult.TypeInfo))
                    {
                        Errors.AddError($"Expected expression of type '{argType.TypeInfo.Name}' instead of '{typeResult.TypeInfo.Name}'.", current.Diagnostics);
                        return (false, null, null);
                    }

                    typedCallArgs.Add(typeResult.NewNode);
                }

                var resultType = FindTypeByName(foundFunction.DesiredType);

                if (!resultType.Found)
                {
                    Errors.AddError($"Type {foundFunction.DesiredType} is not found.", ufce.Diagnostics);
                    return (false, null, null);
                }

                var newExpression = new TypedFunctionCallExpression
                (
                    ufce.Diagnostics,
                    ufce.FunctionName,
                    resultType.TypeInfo,
                    typedCallArgs
                );

                ufce.CopyTo(newExpression);

                return (true, newExpression.TypeInfo, newExpression);
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
