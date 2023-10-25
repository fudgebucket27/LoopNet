using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The offchain fee info
    /// </summary>
    public class OffchainFee
    {
        /// <summary>
        /// The token
        /// </summary>
        [JsonProperty("token")]
        public string? Token { get; set; }
        /// <summary>
        /// The token id
        /// </summary>
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        /// <summary>
        /// The fee
        /// </summary>
        [JsonProperty("fee")]
        public string? Fee { get; set; }
        /// <summary>
        /// The discount, related to VIP levels
        /// </summary>
        [JsonProperty("discount")]
        public decimal Discount { get; set; }
    }
    /// <summary>
    /// The offchain fee API response
    /// </summary>
    public class OffchainFeeResponse
    {
        /// <summary>
        /// The gas price
        /// </summary>
        [JsonProperty("gasPrice")]
        public string? GasPrice { get; set; }
        /// <summary>
        /// The fees
        /// </summary>
        [JsonProperty("fees")]
        public List<OffchainFee>? Fees { get; set; }
    }
}
