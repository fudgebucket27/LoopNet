using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Helpers
{
    public static class LoopNetUtils
    {
        public static BigInteger ParseHexUnsigned(string toParse)
        {
            toParse = toParse.Replace("0x", "");
            var parsResult = BigInteger.Parse(toParse, System.Globalization.NumberStyles.HexNumber);
            if (parsResult < 0)
                parsResult = BigInteger.Parse("0" + toParse, System.Globalization.NumberStyles.HexNumber);
            return parsResult;
        }

        public static decimal MultiplyByPowerOfTen(decimal value, int power)
        {
            if (power < 0)
            {
                throw new ArgumentOutOfRangeException("power", "The power must be a non-negative integer.");
            }

            for (int i = 0; i < power; i++)
            {
                value *= 10m;
            }

            return value;
        }
    }
}
