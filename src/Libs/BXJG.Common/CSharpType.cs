using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Common
{
    public class CSharpType
    {
        public static string String = typeof(string).Name;

        public static readonly string Int = typeof(int).Name;
        public static readonly string Long = typeof(long).Name;
        public static readonly string Byte = typeof(byte).Name;

        public static readonly string Double = typeof(double).Name;
        public static readonly string Float = typeof(float).Name;
        public static readonly string Decimal = typeof(decimal).Name;

        public static readonly string DateTime = typeof(DateTime).Name;
        public static readonly string DateTimeOffset = typeof(DateTimeOffset).Name;

    }
}
