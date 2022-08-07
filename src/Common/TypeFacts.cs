using System.Collections.Generic;

namespace Common
{
    public static class TypeFacts
    {
        public static Dictionary<string, TypeInfo> TypeName2TypeMapper = new()
        {
            { "int", GetInt32() },
            { "long", GetInt64() },
            { "int64", GetInt64() },
            { "double", GetDouble() },
            { "string", GetString() },
            { "void", GetVoid() },
            { "bool", GetBoolean() },
        };

        public static TypeInfo GetBoolean()
        {
            return new TypeInfo("bool");
        }

        public static TypeInfo GetString()
        {
            return new TypeInfo("string");
        }

        public static TypeInfo GetInt64()
        {
            return new TypeInfo("int64");
        }

        public static TypeInfo GetInt32()
        {
            return new TypeInfo("int");
        }

        public static TypeInfo GetDouble()
        {
            return new TypeInfo("double");
        }

        public static TypeInfo GetVoid()
        {
            return new TypeInfo("void");
        }
    }
}
