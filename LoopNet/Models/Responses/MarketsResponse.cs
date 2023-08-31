using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class MarketData
    {
        [JsonProperty("Market")]
        public string? Market { get; set; }
        [JsonProperty("baseTokenId")]
        public int BaseTokenId { get; set; }
        [JsonProperty("quoteTokenId")]
        public int QuoteTokenId { get; set; }
        [JsonProperty("precisionForPrice")]
        public int PrecisionForPrice { get; set; }
        [JsonProperty("orderbookAggLevels")]
        public int OrderbookAggLevels { get; set; }
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    public class MarketsResponse
    {
        [JsonProperty("markets")]
        public List<MarketData>? Markets { get; set; }
    }
}
