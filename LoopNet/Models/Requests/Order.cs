using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    public class Order
    {
        [JsonProperty("exchange")]
        public string? Exchange { get; set; }
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
        [JsonProperty("sellToken")]
        public Token? SellToken { get; set; }
        [JsonProperty("buyToken")]
        public Token? BuyToken { get; set; }
        [JsonProperty("allOrNone")]
        public bool AllOrNone { get; set; } = true; // Currently only supports true
        [JsonProperty("fillAmountBOrS")]
        public bool FillAmountBOrS { get; set; } // Wat?
        [JsonProperty("validUntil")]
        public long ValidUntil { get; set; } // It's a timestamp
        [JsonProperty("maxFeeBips")]
        public int MaxFeeBips { get; set; } = 20; // Maximum order fee that the user can accept, value range (in ten thousandths) 1 ~ 63. WAT???
        [JsonProperty("eddsaSignature")]
        public string? EddsaSignature { get; set; }

        // And now for the optionals
        [JsonProperty("orderType")]
        public string? OrderType { get; set; }    // AMM, LIMIT_ORDER, MAKER_ONLY, TAKER_ONLY
        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
        [JsonProperty("tradeChannel")]
        public string? TradeChannel { get; set; } // ORDER_BOOK, AMM_POOL, MIXED
        [JsonProperty("taker")]
        public string? Taker { get; set; }        // {Used by the P2P order which user specify the taker, so far its} WAAAAAAAAAAAAAAAT?????
        [JsonProperty("poolAddress")]
        public string? PoolAddress { get; set; }  // The AMM pool address if order type is AMM 
        [JsonProperty("affiliate")]
        public string? Affiliate { get; set; }    // This one is very interesting from a profitability standpoint @.@
    }
}
