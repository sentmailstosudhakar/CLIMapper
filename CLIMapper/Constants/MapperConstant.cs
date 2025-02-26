using System;
using System.Globalization;

namespace CLIMapper
{
    internal static class MapperConstant
    {
        /// <summary>
        /// Key Delimiter.
        /// </summary>
        public const char KeyDelimiter = '|';

        /// <summary>
        /// Format Provider.
        /// </summary>
        public static readonly IFormatProvider formatProvider = cultureInfo;

        public static readonly CultureInfo cultureInfo = new CultureInfo("en-US");
    }
}