using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class OrderResponse
    {
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
        [JsonProperty("status")]
        public string? Status { get; set; }
        [JsonProperty("isIdempotent")]
        public bool IsIdempotent { get; set; }
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("tokens")]
        public List<int> Tokens { get; set; }
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}
