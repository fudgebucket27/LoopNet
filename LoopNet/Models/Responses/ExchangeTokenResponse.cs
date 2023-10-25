using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The gas amounts
    /// </summary>
    public class GasAmounts
    {
        /// <summary>
        /// Distribution
        /// </summary>
        [JsonProperty("distribution")]
        public string? Distribution { get; set; }
        /// <summary>
        /// The deposit 
        /// </summary>
        [JsonProperty("deposit")]
        public string? Deposit { get; set; }
    }

    /// <summary>
    /// Red packet amounts
    /// </summary>
    public class LuckyTokenAmounts
    {
        /// <summary>
        /// Minimum red packet amount
        /// </summary>
        [JsonProperty("minimum")]
        public string? Minimum { get; set; }
        /// <summary>
        /// Maximum red packet amount
        /// </summary>
        [JsonProperty("maximum")]
        public string? Maximum { get; set; }
        /// <summary>
        /// Dust red packet amount
        /// </summary>
        [JsonProperty("dust")]
        public string? Dust { get; set; }
    }

    /// <summary>
    /// Order amounts
    /// </summary>
    public class OrderAmounts
    {
        /// <summary>
        /// Minimum order amount
        /// </summary>
        [JsonProperty("minimum")]
        public string? Minimum { get; set; }
        /// <summary>
        /// Maximum order amount
        /// </summary>
        [JsonProperty("maximum")]
        public string? Maximum { get; set; }
        /// <summary>
        /// Dust order amount
        /// </summary>
        [JsonProperty("dust")]
        public string? Dust { get; set; }
    }

    /// <summary>
    /// Exchange token API Response
    /// </summary>
    public class ExchangeTokenResponse
    {
        /// <summary>
        /// The type
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
        /// <summary>
        /// The token id
        /// </summary>
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        /// <summary>
        /// The symbol
        /// </summary>
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
        /// <summary>
        /// The contract address
        /// </summary>
        [JsonProperty("address")]
        public string? Address { get; set; }
        /// <summary>
        /// The decimals
        /// </summary>
        [JsonProperty("decimals")]
        public int Decimals { get; set; }
        /// <summary>
        /// The precision
        /// </summary>
        [JsonProperty("precision")]
        public int Precision { get; set; }
        /// <summary>
        /// The precision for order
        /// </summary>
        [JsonProperty("precisionForOrder")]
        public int PrecisionForOrder { get; set; }
        /// <summary>
        /// The order amounts
        /// </summary>
        [JsonProperty("orderAmounts")]
        public OrderAmounts? OrderAmounts { get; set; }
        /// <summary>
        /// The red packet amounts
        /// </summary>
        [JsonProperty("luckyTokenAmounts")]
        public LuckyTokenAmounts? LuckyTokenAmounts { get; set; }
        /// <summary>
        /// The fast withdrawal limit
        /// </summary>
        [JsonProperty("fastWithdrawLimit")]
        public string? FastWithdrawLimit { get; set; }
        /// <summary>
        /// The gas amounts
        /// </summary>
        [JsonProperty("gasAmounts")]
        public GasAmounts? GasAmounts { get; set; }
        /// <summary>
        /// If it's enabled for trading on Loopring
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

}
