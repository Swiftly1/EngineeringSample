using System;
using System.Collections.Generic;

namespace Common
{
    public static class TypeFacts
    {
        public static Dictionary<string, TypeInfo> TypeName2TypeMapper = new Dictionary<string, TypeInfo>
        {
            { "int", GetInt32() },
            { "int32", GetInt32() },
            { "long", GetInt64() },
            { "int64", GetInt64() },
        };

        private static TypeInfo GetInt64()
        {
            return new TypeInfo
            {
                Name = "int64"
            };
        }

        private static TypeInfo GetInt32()
        {
            return new TypeInfo
            {
                Name = "int32"
            };
        }
    }
}
