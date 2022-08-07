using Common;
using System;
using AST.Trees;
using System.Linq;
using System.Text;
using AST.Trees.Miscs;
using System.Collections.Generic;
using AST.Trees.Statements.Typed;
using AST.Trees.Expressions.Typed;
using AST.Trees.Declarations.Typed;

namespace Emitter.LLVM
{
    public class LLVM_Emitter : BaseEmitter
    {
        private readonly LLVM_ScopeManager _scopeManager = new();

        private readonly StringBuilder _sb = new();

        public LLVM_Emitter(IMessagesPrinter? printer) : base(printer)
        {
        }

        public override Result<string> Emit(Node node)
        {
            try
            {
                InternalEmit(node);
            }
            catch (Exception ex)
            {
                return Result<string>.Error(ex.ToString());
            }

            return Result<string>.Ok(_sb.ToString());
        }

        private void InternalEmit(Node node, int tabDepth = 0)
        {
            if (node is RootNode)
            {
                _scopeManager.AddScope();
                EmitSubNodes(node, tabDepth);
            }
            else if (node is TypedFunctionNode fn)
            {
                var scope = _scopeManager.AddScope();

                scope.RegisterFunctionArg(fn.Arguments.Count);
                var args = LLVM_Helpers.FunctionArgsToLLVM(fn);

                PrintNewLineWrapper($"define dso_local {fn.Type.ToLLVMType()} @{fn.Name}({args})", tabDepth);
                PrintNewLineWrapper("{", tabDepth);

                if (fn.Arguments.Any())
                {
                    RegisterFunctionParameters(fn, scope, tabDepth + 1);
                }

                EmitSubNodes(node, tabDepth + 1);

                if (fn.Type.Name == "void")
                    PrintNewLineWrapper("ret void", tabDepth + 1);

                PrintNewLineWrapper("}", tabDepth);
                PrintNewLineWrapper("", tabDepth);
            }
            else if (node is TypedVariableDeclarationStatement tvar)
            {
                var lastId = TransformExpression(tvar.Expression, tabDepth);
                DeclareVariableWithKnownIndex(tvar.VariableName, _scopeManager.GetLastScope(), lastId);
            }
            else if (node is TypedReturnStatement rsn)
            {
                // TODO: Support VariableUse Expression

                //var scope = _scopeManager.GetLastScope();
                //if (rsn.ReturnExpression is variableuse vue)
                //{
                //    var id = scope.VariableName_NumberDictionary[vue.VariableName];
                //    var type = _helper.TypesToLLVMTypes(vue.TypeKind);
                //    PrintNewLineWrapper($"ret {type} %{id}", tabDepth);
                //}
                //else
                {
                    var lastId = TransformExpression(rsn.ReturnExpression, tabDepth);
                    var type = rsn.ReturnExpression.TypeInfo.ToLLVMType();
                    PrintNewLineWrapper($"ret {type} %{lastId}", tabDepth);
                }
            }
            else if (node is BodyNode)
            {
                EmitSubNodes(node, tabDepth);
            }
            else if (node is TypedIfStatement tifs)
            {
                var lastId = TransformExpression(tifs.Condition);

                var trueBranchId = _scopeManager.GetLastScope().GetNextTrueBranchNumber();
                var falseBranchId = _scopeManager.GetLastScope().GetNextFalseBranchNumber();
                //var endBranchId = _scopeManager.GetLastScope().GetNextEndBranchNumber();

                PrintNewLineWrapper($"br i1 %{lastId}, label %true_branch_{trueBranchId}, label %false_branch_{falseBranchId}", tabDepth);
                PrintNewLineWrapper($"true_branch_{trueBranchId}:", tabDepth);
                EmitSubNodes(tifs.BranchTrue, tabDepth + 1);
                PrintNewLineWrapper($"false_branch_{falseBranchId}:", tabDepth);
                EmitSubNodes(tifs.BranchFalse, tabDepth + 1);
                //PrintNewLineWrapper($"end_branch_{endBranchId}:", tabDepth);
            }
            else if (node is TypedContainerNode tcn)
            {
                PrintNewLineWrapper($"%{tcn.Name} = type", tabDepth);
                PrintNewLineWrapper("{", tabDepth);
                EmitSubNodes(tcn, tabDepth + 1);
                PrintNewLineWrapper("}", tabDepth);
                PrintNewLineWrapper("", tabDepth);
            }
            else if (node is TypedContainerFieldNode tcfn)
            {
                var separator = tcfn.IsLast ? "" : ",";
                PrintNewLineWrapper(tcfn.TypeInfo.ToLLVMType() + separator, tabDepth);
            }
            else if (node is ScopeableNode)
            {
                _scopeManager.AddScope();
                EmitSubNodes(node, tabDepth);
            }
            else
            {
                throw new Exception($"Unsupported Node kind by LLVM IR Emitter - {node.GetType()}");
            }
        }

        private int TransformExpression(TypedExpression expression, int tabDepth = 1)
        {
            if (expression.IsConstant() && expression is ConstantTypedExpression constexpr)
            {
                return AllocateVariable(constexpr, tabDepth);
            }
            else if (expression is ComplexTypedExpression cte)
            {
                var leftId = TransformExpression(cte.Left);
                var rightId = TransformExpression(cte.Right);

                var next = DeclareTemporaryVariable();
                AllocateExpressionVariable(tabDepth, cte, leftId, rightId, next);

                return next;
            }
            else if (expression is TypedFunctionCallExpression tfc)
            {
                var nextLoad = AllocateFunctionCallVariable(tabDepth, tfc);
                return nextLoad;
            }
            else if (expression is TypedVariableUseExpression tvue)
            {
                var variables = _scopeManager.GetLastScope().VariablesWithinScope;

                if (variables.ContainsKey(tvue.VariableName))
                {
                    return variables[tvue.VariableName];
                }
                else
                {
                    throw new Exception("For some reason current scope doesnt have this variable");
                }
            }
            else if (expression is TypedNewExpression tne)
            {
                var name = tne.TypeInfo.Name;
                var addressOfNewObj = DeclareTemporaryVariable();
                PrintNewLineWrapper($"%{addressOfNewObj} = alloca %{name}, align 4", tabDepth);

                foreach (var initializer in tne.InitializationList)
                {
                    var exprValue = TransformExpression(initializer.Expression);
                    var exprType = initializer.Expression.TypeInfo.ToLLVMType();
                    var nextInitializer = DeclareTemporaryVariable();
                    PrintNewLineWrapper($"%{nextInitializer} = getelementptr inbounds " +
                        $"%{name}, %{name}* %{addressOfNewObj}, {exprType} 0, {exprType} {initializer.Index}", tabDepth);
                    PrintNewLineWrapper($"store {exprType} %{exprValue}, {exprType}* %{nextInitializer}, align 4", tabDepth);
                }

                return addressOfNewObj;
            }

            throw new NotImplementedException($"TransformExpression doesn't have implementation for: {expression.GetType()}.");
        }

        private int AllocateFunctionCallVariable(int tabDepth, TypedFunctionCallExpression expr)
        {
            if (expr.TypeInfo == TypeFacts.GetInt32())
            {
                var callArgs = GenerateCallArgs(expr.CallArguments);

                var nextAlloca = DeclareTemporaryVariable();
                var nextCall = DeclareTemporaryVariable();
                var nextLoad = DeclareTemporaryVariable();

                // allocate
                var type = $"{expr.TypeInfo.ToLLVMType()}";
                PrintNewLineWrapper($"%{nextAlloca} = alloca {type}, align 4", tabDepth);

                PrintNewLineWrapper($"%{nextCall} = call {type} @{expr.FunctionName}({callArgs})", tabDepth);
                PrintNewLineWrapper($"store {type} %{nextCall}, {type}* %{nextAlloca}, align 4", tabDepth);
                PrintNewLineWrapper($"%{nextLoad} = load {type}, {type}* %{nextAlloca}, align 4", tabDepth);

                return nextLoad;
            }

            // TODO: remove allocate int workaround with add
            // this code probably can be moved into AllocatVariable function
            throw new NotImplementedException("");
        }

        private void RegisterFunctionParameters(TypedFunctionNode fn, LLVM_ScopeInfo scope, int tabDepth)
        {
            int argIndex = 0;
            foreach (var arg in fn.Arguments)
            {
                var type = arg.TypeInfo.ToLLVMType();
                var nextAlloca = scope.GetNextVariableNumber();
                var nextLoad = scope.GetNextVariableNumber();

                PrintNewLineWrapper($"%{nextAlloca} = alloca {type}, align 4", tabDepth);
                PrintNewLineWrapper($"store {type} %{argIndex++}, {type}* %{nextAlloca}, align 4", tabDepth);
                PrintNewLineWrapper($"%{nextLoad} = load {type}, {type}* %{nextAlloca}, align 4", tabDepth);

                DeclareVariableWithKnownIndex(arg.Name, scope, nextLoad);
            }
        }

        private string GenerateCallArgs(List<TypedExpression> expressions)
        {
            if (!expressions.Any())
            {
                return "";
            }

            var sb = new StringBuilder();

            for (int i = 0; i < expressions.Count; i++)
            {
                var argId = TransformExpression(expressions[i]);

                sb
                    .Append(expressions[i].TypeInfo.ToLLVMType())
                    .Append(' ')
                    .Append('%')
                    .Append(argId);

                if (i < expressions.Count - 1)
                {
                    sb.Append(',');
                }
            }

            return sb.ToString();
        }

        private int AllocateVariable(ConstantTypedExpression expr, int tabDepth)
        {
            if (expr.TypeInfo == TypeFacts.GetInt32())
            {
                var nextAlloca = DeclareTemporaryVariable();
                var nextLoad = DeclareTemporaryVariable();

                var type = expr.TypeInfo.ToLLVMType();
                PrintNewLineWrapper($"%{nextAlloca} = alloca {type}, align 4", tabDepth);
                PrintNewLineWrapper($"store {type} {expr.Value}, {type}* %{nextAlloca}, align 4", tabDepth);
                PrintNewLineWrapper($"%{nextLoad} = load {type}, {type}* %{nextAlloca}, align 4", tabDepth);
                PrintNewLineWrapper("");

                // Previous, soft allocation as done as "x + 0" instead of alloca, store, load.
                //_sink.EmitNewLineWithTabs($"%{next} = add {_helper.TypesToLLVMTypes(expr.TypeKind)} {expr.Value}, 0", tabDepth);

                return nextLoad;
            }
            else if (expr.TypeInfo == TypeFacts.GetBoolean())
            {
                //  % 1 = alloca i8, align 1
                //  store i8 1, i8 * % 1, align 1

                // TODO: store i8 >>1<<?

                var next = DeclareTemporaryVariable();
                PrintNewLineWrapper($"%{next} = alloca {expr.TypeInfo.ToLLVMType()}, align 1", tabDepth);
                PrintNewLineWrapper($"store i8 1, i8* %{next}, align 1", tabDepth);

                return next;
            }

            throw new Exception("unsupported type");
        }

        private void AllocateExpressionVariable(int tabDepth, ComplexTypedExpression expr, int leftId, int rightId, int next)
        {
            if (expr.TypeInfo == TypeFacts.GetInt32())
            {
                var type = expr.TypeInfo.ToLLVMType();
                var op = expr.Operator.OperatorConverter();
                PrintNewLineWrapper($"%{next} = {op} {type} %{leftId}, %{rightId}", tabDepth);
            }
            else if (expr.TypeInfo == TypeFacts.GetBoolean())
            {
                // TODO: %3 = icmp sgt >>i32<< %1, %2
                // comparison requires specifying type of args, so
                // will taking type of just left side be fine?

                var type = expr.Left.TypeInfo.ToLLVMType();
                var op = expr.Operator.OperatorConverter();
                PrintNewLineWrapper($"%{next} = {op} {type} %{leftId}, %{rightId}", tabDepth);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void DeclareVariableWithKnownIndex(string id, LLVM_ScopeInfo scope, int lastId)
        {
            scope.VariablesWithinScope.Add(id, lastId);
        }

        private int DeclareTemporaryVariable()
        {
            var scope = _scopeManager.GetLastScope();
            var next = scope.GetNextVariableNumber();
            scope.VariablesWithinScope.Add($"temp_{next}", next);

            return next;
        }

        private void EmitSubNodes(Node node, int tabDepth)
        {
            foreach (var sub_node in node.Children)
            {
                InternalEmit(sub_node, tabDepth);
            }
        }

        public void PrintNewLineWrapper(string s, int tabDepth = 0)
        {
            var tab = new string('\t', tabDepth);
            _sb.Append(tab).AppendLine(s);

            _printer?.PrintColorNewLine(s, tabDepth);
        }
    }
}
