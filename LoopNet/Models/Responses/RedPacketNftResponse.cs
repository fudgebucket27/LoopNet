using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The red packet response
    /// </summary>
    public class RedPacketMintResponse
    {
        /// <summary>
        /// The hash
        /// </summary>
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        /// <summary>
        /// The status
        /// </summary>
        [JsonProperty("status")]
        public string? Status { get; set; }
        /// <summary>
        /// Is it idempotent?
        /// </summary>
        [JsonProperty("isIdempotent")]
        public bool IsIdempotent { get; set; }
        /// <summary>
        /// The NftData
        /// </summary>
        [JsonProperty("nftData")]
        public string? NftData { get; set; }
        /// <summary>
        /// The account Id
        /// </summary>
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        /// <summary>
        /// The tokenId
        /// </summary>
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        /// <summary>
        /// The storage id
        /// </summary>
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}
