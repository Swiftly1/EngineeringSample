using System;
using System.Linq;
using AST.Builders;
using Common;
using Common.Lexing;
using Text2Abstraction.LexicalElements;

namespace AST
{
    internal static class ASTHelper
    {
        //public static bool MatchesThose(this MovableLookup<LexElement> movable, params LexingElement[] items)
        //{
        //    var result = movable.TryGetAhead(items.Length);

        //    if (!result.Sucess)
        //        return false;

        //    return result.Items.Select(x => x.Kind).SequenceEqual(items);
        //}
    }
}
