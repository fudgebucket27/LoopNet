using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    public class Token
    {
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        [JsonProperty("volume")]
        public string? Volume { get; set; }

        /// <summary>
        /// Creates a token
        /// </summary>
        /// <param name="tokenId">The tokenId on Loopring</param>
        /// <param name="amount">The amount of the token</param>
        /// <param name="decimals">The amount of decimals</param>
        public Token(int tokenId,  decimal amount, int decimals)
        {
            TokenId = tokenId;
            Volume = CalculateTokenVolume(amount, decimals); 
        }

        public Token(){}

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
    }
}
