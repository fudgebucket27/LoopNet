using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The order API response
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// The hash
        /// </summary>
        [JsonProperty("hash")]
        public string? Hash { get; set; }
        /// <summary>
        /// The client order id
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
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
        /// The tokens
        /// </summary>
        [JsonProperty("tokens")]
        public List<int>? Tokens { get; set; }
        /// <summary>
        /// The storage id
        /// </summary>
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
    }
}
