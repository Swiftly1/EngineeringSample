using System.Collections.Generic;

namespace Common
{
    public static class TypeFacts
    {
        public static Dictionary<string, TypeInfo> TypeName2TypeMapper = new()
        {
            { "int", GetInt32() },
            { "int32", GetInt32() },
            { "long", GetInt64() },
            { "int64", GetInt64() },
            { "double", GetDouble() },
            { "string", GetString() },
            { "void", GetVoid() },
        };

        public static TypeInfo GetString()
        {
            return new TypeInfo
            {
                Name = "string"
            };
        }

        public static TypeInfo GetInt64()
        {
            return new TypeInfo
            {
                Name = "int64"
            };
        }

        public static TypeInfo GetInt32()
        {
            return new TypeInfo
            {
                Name = "int32"
            };
        }

        public static TypeInfo GetDouble()
        {
            return new TypeInfo
            {
                Name = "double"
            };
        }

        public static TypeInfo GetVoid()
        {
            return new TypeInfo
            {
                Name = "void"
            };
        }
    }
}
