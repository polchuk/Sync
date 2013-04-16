using System;
using System.Globalization;

namespace Sync
{
    public static class Utils
    {
        public static string Format(this DateTime dt)
        {
            return dt.ToString(CultureInfo.InvariantCulture).Substring(11, 8);
        }

        public static string Format(this Guid x)
        {
            return x.ToString().Substring(0, 8);
        }

        public static string Format(this bool x)
        {
            return x ? "Y" : "N";
        }
    }
}
