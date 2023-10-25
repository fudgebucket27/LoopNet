using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The market data
    /// </summary>
    public class MarketData
    {
        /// <summary>
        /// The market
        /// </summary>
        [JsonProperty("Market")]
        public string? Market { get; set; }
        /// <summary>
        /// The base token id
        /// </summary>
        [JsonProperty("baseTokenId")]
        public int BaseTokenId { get; set; }
        /// <summary>
        /// The quote token id
        /// </summary>
        [JsonProperty("quoteTokenId")]
        public int QuoteTokenId { get; set; }
        /// <summary>
        /// The precision for price
        /// </summary>
        [JsonProperty("precisionForPrice")]
        public int PrecisionForPrice { get; set; }
        /// <summary>
        /// The order book agg levels
        /// </summary>
        [JsonProperty("orderbookAggLevels")]
        public int OrderbookAggLevels { get; set; }
        /// <summary>
        /// If it's enabled for trading
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }
    }

    /// <summary>
    /// The markets API response
    /// </summary>
    public class MarketsResponse
    {
        /// <summary>
        /// The markets
        /// </summary>
        [JsonProperty("markets")]
        public List<MarketData>? Markets { get; set; }
    }
}
