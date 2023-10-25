using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The amount
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// The token symbol
        /// </summary>
        [JsonProperty("tokenSymbol")]
        public string? TokenSymbol { get; set; }
        /// <summary>
        /// Base Order Info
        /// </summary>
        [JsonProperty("baseOrderInfo")]
        public BaseOrderInfo? BaseOrderInfo { get; set; }
        /// <summary>
        /// User Order Info
        /// </summary>
        [JsonProperty("userOrderInfo")]
        public UserOrderInfo? UserOrderInfo { get; set; }
        /// <summary>
        /// Trade Cost
        /// </summary>
        [JsonProperty("tradeCost")]
        public string? TradeCost { get; set; }
    }

    /// <summary>
    /// Base Order Info
    /// </summary>
    public class BaseOrderInfo
    {
        /// <summary>
        /// The minimum amount
        /// </summary>
        [JsonProperty("minAmount")]
        public string? MinAmount { get; set; }
        /// <summary>
        /// The maker rate
        /// </summary>
        [JsonProperty("makerRate")]
        public int MakerRate { get; set; }
        /// <summary>
        /// The taker rate
        /// </summary>
        [JsonProperty("takerRate")]
        public int TakerRate { get; set; }
    }

    /// <summary>
    /// The order user rate amount
    /// </summary>
    public class OrderUserRateAmountResponse
    {
        /// <summary>
        /// Gas price
        /// </summary>
        [JsonProperty("gasPrice")]
        public string? GasPrice { get; set; }
        /// <summary>
        /// Amounts
        /// </summary>
        [JsonProperty("amounts")]
        public List<Amount>? Amounts { get; set; }
        /// <summary>
        /// When cache is overdue
        /// </summary>
        [JsonProperty("cacheOverdueAt")]
        public int CacheOverdueAt { get; set; }
    }

    /// <summary>
    /// User order info
    /// </summary>
    public class UserOrderInfo
    {
        /// <summary>
        /// The minimum amount
        /// </summary>
        [JsonProperty("minAmount")]
        public string? MinAmount { get; set; }
        /// <summary>
        /// The maker rate
        /// </summary>
        [JsonProperty("makerRate")]
        public int MakerRate { get; set; }
        /// <summary>
        /// The taker rate
        /// </summary>
        [JsonProperty("takerRate")]
        public int TakerRate { get; set; }
    }


}
