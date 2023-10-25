using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// Nft holder info
    /// </summary>
    public class NftHolder
    {
        /// <summary>
        /// The account id
        /// </summary>
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        /// <summary>
        /// The address
        /// </summary>
        [JsonProperty("address")]
        public string? Address { get; set; }
        /// <summary>
        /// The token id
        /// </summary>
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        /// <summary>
        /// Amount held
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
    /// <summary>
    /// Nft holder API response
    /// </summary>
    public class NftHoldersResponse
    {
        /// <summary>
        /// The total number held by all holders
        /// </summary>
        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }
        /// <summary>
        /// The nft holders
        /// </summary>
        [JsonProperty("nftHolders")]
        public List<NftHolder>? NftHolders { get; set; }
    }
}
