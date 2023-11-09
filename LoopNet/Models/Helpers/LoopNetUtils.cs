using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LoopNet.Models.Helpers
{
    /// <summary>
    /// Assortment of utility methods used by LoopNet
    /// </summary>
    public static class LoopNetUtils
    {
        /// <summary>
        /// Parse the Hex To BigInteger
        /// </summary>
        /// <param name="toParse">The string to parse</param>
        /// <returns></returns>
        public static BigInteger ParseHexUnsigned(string toParse)
        {
            toParse = toParse.Replace("0x", "");
            var parsResult = BigInteger.Parse(toParse, System.Globalization.NumberStyles.HexNumber);
            if (parsResult < 0)
                parsResult = BigInteger.Parse("0" + toParse, System.Globalization.NumberStyles.HexNumber);
            return parsResult;
        }

        /// <summary>
        /// Calculates the token volume string
        /// </summary>
        /// <param name="value">The decimal amount of the token</param>
        /// <param name="decimals">The amount of decimals</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the decimals is negative integer</exception>
        public static string CalculateTokenVolume(decimal value, int decimals)
        {
            if (decimals < 0)
            {
                throw new ArgumentOutOfRangeException("power", "The decimals must be a non-negative integer.");
            }

            for (int i = 0; i < decimals; i++)
            {
                value *= 10m;
            }

            return value.ToString("F0");
        }

        /// <summary>
        /// Encodes a URL
        /// </summary>
        /// <param name="stringToEncode">The url to encode</param>
        /// <returns></returns>
        public static string UrlEncodeUpperCase(string stringToEncode)
        {
            var reg = new Regex(@"%[a-f0-9]{2}");
            stringToEncode = HttpUtility.UrlEncode(stringToEncode);
            return reg.Replace(stringToEncode, m => m.Value.ToUpperInvariant());
        }
    }
}
