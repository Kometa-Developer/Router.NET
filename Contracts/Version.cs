using System;

namespace Router.Contracts
{
    public readonly struct Version : IEquatable<Version>, IComparable<Version>
    {
        private const char Separator = '.';

        public int A { get; }

        public int B { get; }

        public int C { get; }

        public Version(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        public static Version Parse(string source)
        {
            // Empty or null string considered as an infinity
            if (string.IsNullOrEmpty(source))
            {
                return new Version(int.MaxValue, int.MaxValue, int.MaxValue);
            }
            
            var split = source.Split(new[] {Separator}, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 3)
            {
                throw new ArgumentException("Input string format is incorrect. Should be 'X.X.X'");
            }

            return new Version(
                int.Parse(split[0]),
                int.Parse(split[1]),
                int.Parse(split[2])
            );
        }

        public static bool TryParse(string source, out Version result)
        {
            try
            {
                result = Parse(source);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public bool Equals(Version other)
        {
            return A == other.A && B == other.B && C == other.C;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Version other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hashCode = A;

            hashCode = (hashCode * 397) ^ B;
            hashCode = (hashCode * 397) ^ C;

            return hashCode;
        }

        public int CompareTo(Version other)
        {
            var aComparison = A.CompareTo(other.A);
            if (aComparison != 0)
            {
                return aComparison;
            }

            var bComparison = B.CompareTo(other.B);
            if (bComparison != 0)
            {
                return bComparison;
            }

            return C.CompareTo(other.C);
        }

        public override string ToString()
        {
            return $"{A}{Separator}{B}{Separator}{C}";
        }

        public static bool operator ==(Version left, Version right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Version left, Version right)
        {
            return !Equals(left, right);
        }

        public static bool operator <(Version left, Version right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Version left, Version right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}