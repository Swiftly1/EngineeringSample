using Common;
using System.Linq;
using Common.Lexing;
using AST.Trees.Expressions;
using System.Collections.Generic;
using Text2Abstraction.LexicalElements;

namespace AST.Trees.Miscs
{
    internal static class FunctionHelpers
    {
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
                        if (i + 1 < data.Count && data[i + 1].Kind == LexingElement.Comma)
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
