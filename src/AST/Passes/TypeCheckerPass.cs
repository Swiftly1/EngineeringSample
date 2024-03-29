﻿using System;
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
            TryToMapUntypedNodeToTyped(root);
            return new TypeCheckerPassResult(PassName, Errors.DumpErrors().ToList());
        }

        // TODO: Would be nice if error handling was done better here
        // e.g by using Result classes

        private void TryToMapUntypedNodeToTyped(Node root)
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
                var usesVar = vds.DesiredType == LanguageFacts.Var;

                if (!result.Found && !usesVar)
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
                if (!usesVar && result.TypeInfo != exprType.TypeInfo)
                {
                    Errors.AddError($"Cannot assign type {exprType.TypeInfo.Name} to {result.TypeInfo.Name}.", current.Diagnostics);
                    return (true, null);
                }

                var newNode = new TypedVariableDeclarationStatement(vds.VariableName, exprType.NewNode, exprType.TypeInfo, vds.ScopeContext, vds.Diagnostics);

                var declaration = newNode.ScopeContext.DeclaredVariablesList.First(x => x.VariableName == vds.VariableName && x.TypeName == type);
                declaration.IsConstant = exprType.NewNode.IsConstant();

                if (usesVar)
                {
                    declaration.TypeName = exprType.TypeInfo.Name;
                }

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
                        // in order to get more error messages we don't return yet.
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
                    ufn.AccessibilityModifier
                );

                return (false, newNode);
            }
            else if (current is UntypedIfStatement uifs)
            {
                var exprType = GenerateBoundedTreeAndGetType(uifs.Condition);

                if (!exprType.Found)
                {
                    return (true, null);
                }

                // TODO: refactor this cuz it's ugly
                TryToMapUntypedNodeToTyped(uifs.BranchTrue);
                TryToMapUntypedNodeToTyped(uifs.BranchFalse);

                var newNode = new TypedIfStatement(exprType.NewNode, uifs.BranchTrue, uifs.BranchFalse, uifs.Diagnostics);
                return (true, newNode);
            }
            else if (current is UntypedAssignmentStatement uas)
            {
                var bounded = GenerateBoundedTreeAndGetType(uas.Expression);

                if (!bounded.Found)
                {
                    return (true, null);
                }

                var newNode = new TypedAssignmentStatement(uas.Name, bounded.NewNode, uas.Diagnostics);

                return (true, newNode);
            }
            else if (current is UntypedContainerNode ucn)
            {
                var typedFields = new List<TypedContainerFieldNode>();

                for (int i = 0; i < ucn.Fields.Count; i++)
                {
                    var field = ucn.Fields[i];
                    var fieldTypeResult = FindTypeByName(field.DesiredType);

                    if (!fieldTypeResult.Found)
                    {
                        Errors.AddError($"Type {field.DesiredType} is not found.", field.TypeDiagnostic);
                    }
                    else
                    {
                        typedFields.Add(new TypedContainerFieldNode
                        (
                            field.Name,
                            fieldTypeResult.TypeInfo,
                            field.NameDiagnostic,
                            field.TypeDiagnostic,
                            i,
                            i == ucn.Fields.Count - 1
                        ));
                    }
                }

                if (ucn.Fields.Count != typedFields.Count)
                    return (true, null);

                var newNode = new TypedContainerNode(ucn.Diagnostics, ucn.Name, ucn.AccessibilityModifier, ucn.ScopeContext, typedFields);
                return (true, newNode);
            }
            else if (current is RootNode)
            {
                return (false, null);
            }
            else if (current is BodyNode)
            {
                return (false, null);
            }
            else if (current is NamespaceNode)
            {
                return (false, null);
            }
            else
            {
                throw new Exception($"Unsupported Node - {current.GetType()}");
            }
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
                    var newExpr = new ConstantTypedExpression(expression.Diagnostics, value, type, cme.ScopeContext);
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

                var found = OperatorFacts.TryMatch(leftResult.TypeInfo, rightResult.TypeInfo, cue.Operator);

                if (found is null)
                {
                    Errors.AddError($"Undefined operator - '{cue.Operator}' between '{leftResult.TypeInfo}' and '{rightResult.TypeInfo}'", cue.Diagnostics);
                    return (false, null, null);
                }

                var newExpression = new ComplexTypedExpression
                (
                    leftResult.NewNode,
                    rightResult.NewNode,
                    cue.Operator,
                    found.ResultType,
                    cue.Diagnostics,
                    cue.ScopeContext
                );

                cue.CopyTo(newExpression);

                return (true, found.ResultType, newExpression);
            }
            else if (expression is ConstantUntypedStringExpression cuse)
            {
                var newExpression = new ConstantTypedExpression(cuse.Diagnostics, cuse.Value, TypeFacts.GetString(), cuse.ScopeContext);

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
                    typedCallArgs,
                    ufce.ScopeContext
                );

                ufce.CopyTo(newExpression);

                return (true, newExpression.TypeInfo, newExpression);
            }
            else if (expression is UntypedVariableUseExpression uvue)
            {
                var found = TryFindVariableInScope(uvue.ScopeContext, uvue.VariableName);

                if (!found.Success)
                {
                    Errors.AddError(found.Message, uvue.Diagnostics);
                    return (false, null, null);
                }

                var newExpr = new TypedVariableUseExpression(uvue.VariableName, found.Data.ScopeDescription.IsConstant, found.Data.TypeInfo, uvue.ScopeContext, uvue.Diagnostics);

                uvue.CopyTo(newExpr);

                return (true, newExpr.TypeInfo, newExpr);
            }
            else if (expression is UntypedNewExpression une)
            {
                var resultType = FindTypeByName(une.DesiredType);

                if (!resultType.Found)
                {
                    Errors.AddError($"Type {une.DesiredType} is not found.", une.Diagnostics);
                    return (false, null, null);
                }

                var typedList = new List<TypedObjectInitializationParam>();

                foreach (var param in une.InitializationList)
                {
                    var typedExpr = GenerateBoundedTreeAndGetType(param.Expression);

                    if (!typedExpr.Found)
                    {
                        Errors.AddError($"Unable to resolve expression for param {param.Name}", param.Diagnostics);
                        return (false, null, null);
                    }

                    typedList.Add(new TypedObjectInitializationParam
                    (
                        param.Name,
                        typedExpr.NewNode,
                        param.Diagnostics,
                        param.Index,
                        param.IsLast
                    ));
                }

                var expectedType = resultType.TypeInfo as InitializableTypeInfo;

                if (typedList.Count < expectedType.InitializationTypesOrdered.Count)
                {
                    Errors.AddError($"You need to initialize all properties.", une.Diagnostics);
                    return (false, null, null);
                }

                if (typedList.Count > expectedType.InitializationTypesOrdered.Count)
                {
                    Errors.AddError($"You're initializing more properties than the object actually has.", une.Diagnostics);
                    return (false, null, null);
                }

                for (int i = 0; i < typedList.Count; i++)
                {
                    var typed = typedList[i];
                    if (typed.Expression.TypeInfo.Name != expectedType.InitializationTypesOrdered[i].DesiredType)
                    {
                        Errors.AddError($"Incorrect type on object initialization list for name {typed.Name}", typed.Diagnostics);
                        return (false, null, null);
                    }
                }

                var newExpr = new TypedNewExpression(une.Diagnostics, resultType.TypeInfo, typedList, une.ScopeContext);
                une.CopyTo(newExpr);
                return (true, newExpr.TypeInfo, newExpr);
            }
            else if (expression is UntypedPropertyUsageExpression upue)
            {
                var found = TryFindVariableInScope(upue.ScopeContext, upue.VariableName);

                if (!found.Success)
                {
                    Errors.AddError(found.Message, upue.Diagnostics);
                    return (false, null, null);
                }

                var initializationList = found.Data.TypeInfo as InitializableTypeInfo;

                ((string Name, string DesiredType) Details, int Index)? propertyInfo = 
                initializationList
                .InitializationTypesOrdered
                .Select((x, i) => (Value: x, Index: i))
                .FirstOrDefault(x => x.Value.Name == upue.PropertyName);

                if (propertyInfo == null)
                {
                    var msg = $"Property with name: '{upue.PropertyName}' does not exist in type '{found.Data.TypeInfo.Name}'.";
                    Errors.AddError(msg, upue.Diagnostics);
                    return (false, null, null);
                }

                var type = FindTypeByName(propertyInfo.Value.Details.DesiredType);

                if (!type.Found)
                {
                    var msg = $"Unable to resolve type of: '{upue.PropertyName}' for type '{found.Data.TypeInfo.Name}'.";
                    Errors.AddError(msg, upue.Diagnostics);
                    return (false, null, null);
                }

                var newExpr = new TypedPropertyUsageExpression
                (
                    upue.Diagnostics,
                    found.Data.TypeInfo,
                    type.TypeInfo,
                    upue.VariableName,
                    upue.PropertyName,
                    propertyInfo.Value.Index,
                    upue.ScopeContext
                );
                upue.CopyTo(newExpr);
                return (true, found.Data.TypeInfo, newExpr);
            }

            throw new NotImplementedException($"Node type {expression.GetType()} is not handled");
        }

        private Result<(TypeInfo TypeInfo, BasicVariableDescription ScopeDescription)> TryFindVariableInScope(UntypedScopeContext scopeContext, string variableName)
        {
            var found = scopeContext.DeclaredVariablesList.FirstOrDefault(x => x.VariableName == variableName);

            if (found is not null)
            {
                var typeResult = FindTypeByName(found.TypeName);

                if (typeResult.Found)
                {
                    return Result<(TypeInfo TypeInfo, BasicVariableDescription ScopeDescription)>.Ok((typeResult.TypeInfo, found));
                }
                else
                {
                    return Result<(TypeInfo TypeInfo, BasicVariableDescription ScopeDescription)>.Error($"Type '{found.TypeName}' for variable with name '{variableName}' is not found.");
                }
            }

            return Result<(TypeInfo TypeInfo, BasicVariableDescription ScopeDescription)>.Error($"Variable with name '{variableName}' does not exist in this scope.");
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
