using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The storage id API response
    /// </summary>
    public class StorageIdResponse
    {
        /// <summary>
        /// The order id
        /// </summary>
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        /// <summary>
        /// The offchain id
        /// </summary>
        [JsonProperty("offchainId")]
        public int OffchainId { get; set; }
    }
}
