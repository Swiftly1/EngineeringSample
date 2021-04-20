namespace Common
{
    public class TypeInfo
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Type: {Name}";
        }
    }
}