using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class GasAmounts
    {
        [JsonProperty("distribution")]
        public string? Distribution { get; set; }
        [JsonProperty("deposit")]
        public string? Deposit { get; set; }
    }

    public class LuckyTokenAmounts
    {
        [JsonProperty("minimum")]
        public string? Minimum { get; set; }
        [JsonProperty("maximum")]
        public string? Maximum { get; set; }
        [JsonProperty("dust")]
        public string? Dust { get; set; }
    }

    public class OrderAmounts
    {
        [JsonProperty("minimum")]
        public string? Minimum { get; set; }
        [JsonProperty("maximum")]
        public string? Maximum { get; set; }
        [JsonProperty("dust")]
        public string? Dust { get; set; }
    }

    public class ExchangeTokenResponse
    {
        [JsonProperty("type")]
        public string? Type { get; set; }
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        [JsonProperty("symbol")]
        public string? Symbol { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("address")]
        public string? Address { get; set; }
        [JsonProperty("decimals")]
        public int Decimals { get; set; }
        [JsonProperty("precision")]
        public int Precision { get; set; }
        [JsonProperty("precisionForOrder")]
        public int PrecisionForOrder { get; set; }
        [JsonProperty("orderAmounts")]
        public OrderAmounts? OrderAmounts { get; set; }
        [JsonProperty("luckyTokenAmounts")]
        public LuckyTokenAmounts? LuckyTokenAmounts { get; set; }
        [JsonProperty("fastWithdrawLimit")]
        public string? FastWithdrawLimit { get; set; }
        [JsonProperty("gasAmounts")]
        public GasAmounts? GasAmounts { get; set; }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

}
