using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class PostNftMintResponse
    {
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        [JsonProperty("nftTokenId")]
        public int NftTokenId { get; set; }
        [JsonProperty("nftId")]
        public string? NftId { get; set; }
        [JsonProperty("nftData")]
        public string? NftData { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("isIdempotent")]
        public bool IsIdempotent { get; set; }
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}
