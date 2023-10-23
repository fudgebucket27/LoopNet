using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class Amount
    {
        [JsonProperty("tokenSymbol")]
        public string? TokenSymbol { get; set; }
        [JsonProperty("baseOrderInfo")]
        public BaseOrderInfo? BaseOrderInfo { get; set; }
        [JsonProperty("userOrderInfo")]
        public UserOrderInfo? UserOrderInfo { get; set; }
        [JsonProperty("tradeCost")]
        public string? TradeCost { get; set; }
    }

    public class BaseOrderInfo
    {
        [JsonProperty("minAmount")]
        public string? MinAmount { get; set; }
         
        [JsonProperty("takerRate")]
        public int MakerRate { get; set; }
        [JsonProperty("takerRate")]
        public int TakerRate { get; set; }
    }

    public class OrderUserRateAmountResponse
    {
        [JsonProperty("gasPrice")]
        public string? GasPrice { get; set; }
        [JsonProperty("amounts")]
        public List<Amount>? Amounts { get; set; }
        [JsonProperty("cacheOverdueAt")]
        public int CacheOverdueAt { get; set; }
    }

    public class UserOrderInfo
    {
        [JsonProperty("minAmount")]
        public string? MinAmount { get; set; }
        [JsonProperty("makerRate")]
        public int MakerRate { get; set; }
        [JsonProperty("takerRate")]

        public int TakerRate { get; set; }
    }


}
