namespace AST.Trees.Miscs
{
    public static class NodeExtensions
    {
        public static bool TryGetProperty(this Node node, string key, out object value)
        {
            if (node.Properties.ContainsKey(key))
            {
                value = node[key];
                return true;
            }

            value = null;
            return false;
        }
    }
}
