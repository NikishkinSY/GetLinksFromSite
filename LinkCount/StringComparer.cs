using System;
using System.Collections.Generic;

namespace LinkCount
{
    public class StringComparer : IEqualityComparer<string>
    {
        public bool Equals(string value1, string value2)
        {
            return string.Equals(value1, value2, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(string value)
        {
            return value.GetHashCode();
        }
    }
}
