using Common;
using System.Linq;
using Common.Lexing;
using AST.Trees.Expressions;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;
using AST.Trees.Expressions.Untyped;

namespace AST.Trees.Miscs
{
    internal static class ExtractionHelpers
    {
        internal static Result<List<ContainerFieldNode>> ExtractContainerFieldList(List<LexElement> data)
        {
            var withoutComma = data.Where((x, i) => (i + 1) % 3 != 0).ToList();

            if (withoutComma.Count % 2 != 0)
            {
                return Result<List<ContainerFieldNode>>.Error("Every field should be made of type name and its name.");
            }

            var fields = new List<ContainerFieldNode>();

            for (int i = 0; i < withoutComma.Count; i += 2)
            {
                var type = withoutComma[i] as LexKeyword;
                var fieldName = withoutComma[i + 1] as LexWord;
                var field = new ContainerFieldNode(fieldName.Value, type.Value, fieldName.Diagnostics, type.Diagnostics);
                fields.Add(field);
            }

            return Result<List<ContainerFieldNode>>.Ok(fields);
        }

        internal static Result<List<Argument>> ExtractFunctionParametersInfo(List<LexElement> data)
        {
            var withoutComma = data.Where((x, i) => (i + 1) % 3 != 0).ToList();

            if (withoutComma.Count % 2 != 0)
            {
                return Result<List<Argument>>.Error("Every argument should be made of type name and its name.");
            }

            var args = new List<Argument>();

            for (int i = 0; i < withoutComma.Count; i += 2)
            {
                string typeName = withoutComma[i] as LexKeyword;
                var argName = withoutComma[i + 1] as LexWord;
                var arg = new Argument(typeName, argName.Value, argName.Diagnostics);

                args.Add(arg);
            }

            return Result<List<Argument>>.Ok(args);
        }

        internal static Result<List<Expression>> ExtractFunctionCallParameters(List<LexElement> data, UntypedScopeContext scopeContext)
        {
            var output = new List<Expression>();

            // states: 0 = expression, 1 comma
            var state = 0;

            for (int i = 0; i < data.Count; i++)
            {
                var current = data[i];

                if (state == 0)
                {
                    if (current.Kind == LexingElement.Comma)
                    {
                        return Result<List<Expression>>.Error($"Unexpected comma at: {current.Diagnostics}");
                    }
                    else
                    {
                        ExpressionBuilder builder = null;
                        // simple expression
                        if (i == data.Count - 1 || (i + 1 < data.Count && data[i + 1].Kind == LexingElement.Comma))
                        {
                            builder = new ExpressionBuilder(new List<LexElement> { current }, scopeContext);
                        }
                        else
                        {
                            // Nested cases like: int test = 345645 + Test(5,Test(5,6), 6);

                            var elements = GetElementsThatMayContainCommas(ref i, data);

                            builder = new ExpressionBuilder(elements, scopeContext);
                        }

                        var result = builder.Build();

                        if (!result.Success)
                            return Result<List<Expression>>.Error(result.Messages);

                        output.Add(result.Data);
                    }

                    state = 1;
                }
                else if (state == 1)
                {
                    state = 0;
                }
            }

            return Result<List<Expression>>.Ok(output);
        }

        internal static Result<List<UntypedObjectInitializationParam>> ExtractObjectInitializationValues(List<LexElement> data, UntypedScopeContext scopeContext)
        {
            var output = new List<UntypedObjectInitializationParam>();

            // states: 0 = name, 1 = equal sign, 2 = expression, 3 = comma
            var state = 0;

            LexWord name = null;
            UntypedExpression expr = null;
            var counter = 0;

            void Add()
            {
                var initializationObject =
                new UntypedObjectInitializationParam
                (
                    name.Value,
                    expr,
                    name.Diagnostics,
                    counter++,
                    false
                );
                output.Add(initializationObject);
                state = 0;
                name = null;
                expr = null;
            }

            for (int i = 0; i < data.Count; i++)
            {
                var current = data[i];

                if (state == 0)
                {
                    name = (current as LexWord);
                    state = 1;
                }
                else if (state == 1)
                {
                    state = 2;
                    continue;
                }
                else if (state == 2)
                {
                    if (current.Kind == LexingElement.Comma)
                    {
                        return Result<List<UntypedObjectInitializationParam>>.Error($"Unexpected comma at: {current.Diagnostics}");
                    }
                    else
                    {
                        ExpressionBuilder builder = null;
                        // simple expression
                        if (i == data.Count - 1 || (i + 1 < data.Count && data[i + 1].Kind == LexingElement.Comma))
                        {
                            builder = new ExpressionBuilder(new List<LexElement> { current }, scopeContext);
                        }
                        else
                        {
                            // Nested cases like: int test = 345645 + Test(5,Test(5,6), 6);

                            var elements = GetElementsThatMayContainCommas(ref i, data);

                            builder = new ExpressionBuilder(elements, scopeContext);
                        }

                        var result = builder.Build();

                        if (!result.Success)
                            return Result<List<UntypedObjectInitializationParam>>.Error(result.Messages);

                        expr = result.Data;
                    }

                    state = 3;

                    // if last
                    if (i == data.Count - 1)
                    {
                        Add();
                    }
                }
                else if (state == 3)
                {
                    Add();
                }
            }

            output.Last().IsLast = true;
            return Result<List<UntypedObjectInitializationParam>>.Ok(output);
        }

        private static List<LexElement> GetElementsThatMayContainCommas(ref int index, List<LexElement> data)
        {
            var list = new List<LexElement>();

            var opened = 0;

            for (; index < data.Count; index++)
            {
                var current = data[index];

                if (current.Kind == LexingElement.OpenParenthesis)
                {
                    opened++;
                }
                else if(current.Kind == LexingElement.ClosedParenthesis)
                {
                    opened--;
                }
                else if (current.Kind == LexingElement.Comma)
                {
                    if (opened == 0)
                    {
                        // going at previous position before returning after collecting all elements
                        index--;
                        break;
                    }
                }

                list.Add(current);
            }

            return list;
        }
    }
}
