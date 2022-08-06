using System.Collections.Generic;

namespace Common
{
    public class InitializableTypeInfo : TypeInfo
    {
        public InitializableTypeInfo(string name) : base(name)
        {
        }

        public List<string> InitializationTypesOrdered { get; set; } = new List<string>();
    }
}