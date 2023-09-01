using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class TransferTokenResponse
    {
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("isIdempotent")]
        public bool IsIdempotent { get; set; }
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}
