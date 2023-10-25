using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    /// <summary>
    /// The order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// The exchange address
        /// </summary>
        [JsonProperty("exchange")]
        public string? Exchange { get; set; }
        /// <summary>
        /// The account id
        /// </summary>
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        /// <summary>
        /// The storage id
        /// </summary>
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
        /// <summary>
        /// The sell token
        /// </summary>
        [JsonProperty("sellToken")]
        public Token? SellToken { get; set; }
        /// <summary>
        /// The buy token
        /// </summary>
        [JsonProperty("buyToken")]
        public Token? BuyToken { get; set; }
        /// <summary>
        /// All or none, current only supports true
        /// </summary>
        [JsonProperty("allOrNone")]
        public bool AllOrNone { get; set; } = true;
        /// <summary>
        /// To fill amount by buy or sell
        /// </summary>
        [JsonProperty("fillAmountBOrS")]
        public bool FillAmountBOrS { get; set; } 
        /// <summary>
        /// Unix timestamp in seconds for the expiry
        /// </summary>
        [JsonProperty("validUntil")]
        public long ValidUntil { get; set; }
        /// <summary>
        /// Maximum order fee that the user can accept, between 1 - 63
        /// </summary>
        [JsonProperty("maxFeeBips")]
        public int MaxFeeBips { get; set; } = 20;
        /// <summary>
        /// Eddsa signature
        /// </summary>
        [JsonProperty("eddsaSignature")]
        public string? EddsaSignature { get; set; }

        /// <summary>
        /// Order type - AMM, LIMIT_ORDER, MAKER_ONLY, TAKER_ONLY. OPTIONAL
        /// </summary>
        [JsonProperty("orderType")]
        public string? OrderType { get; set; }    
        /// <summary>
        /// Client order id, arbitrary set by client. OPTIONAL
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// Channel to trade on. ORDER_BOOK,AMM_POOL, MIXED. OPTIONAL
        /// </summary>
        [JsonProperty("tradeChannel")]
        public string? TradeChannel { get; set; }
        /// <summary>
        /// Used by the P2P order which user specify the taker
        /// </summary>
        [JsonProperty("taker")]
        public string? Taker { get; set; }
        /// <summary>
        /// The AMM pool address if order type is AMM 
        /// </summary>
        [JsonProperty("poolAddress")]
        public string? PoolAddress { get; set; }
        /// <summary>
        /// The affiliate
        /// </summary>
        [JsonProperty("affiliate")]
        public string? Affiliate { get; set; } 
    }
}
