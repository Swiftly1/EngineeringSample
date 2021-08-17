using System;

namespace Common
{
    public class TypeInfo : IEquatable<TypeInfo>
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return $"Type: {Name}";
        }

        public bool Equals(TypeInfo other)
        {
            if (other is null)
                return false;

            return this.Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypeInfo other))
            {
                return false;
            }

            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }

        public static bool operator ==(TypeInfo left, TypeInfo right)
        {
            if (left is null || right is null)
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(TypeInfo left, TypeInfo right)
        {
            return !(left == right);
        }
    }
}