using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The post nft mint API response
    /// </summary>
    public class PostNftMintResponse
    {
        /// <summary>
        /// The hash
        /// </summary>
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        /// <summary>
        /// The nft token id
        /// </summary>
        [JsonProperty("nftTokenId")]
        public int NftTokenId { get; set; }
        /// <summary>
        /// The nft id
        /// </summary>
        [JsonProperty("nftId")]
        public string? NftId { get; set; }
        /// <summary>
        /// The nft data
        /// </summary>
        [JsonProperty("nftData")]
        public string? NftData { get; set; }
        /// <summary>
        /// The status
        /// </summary>
        [JsonProperty("status")]
        public string? Status { get; set; }
        /// <summary>
        /// If idempotent
        /// </summary>
        [JsonProperty("isIdempotent")]
        public bool IsIdempotent { get; set; }
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
    }
}
