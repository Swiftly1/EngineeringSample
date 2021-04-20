using Common.Lexing;

namespace AST.Miscs.Matching
{
    public static class MatcherUtils
    {
        public static Matcher Match(params LexingElement[] items)
        {
            return new Matcher(items);
        }
    }
}
