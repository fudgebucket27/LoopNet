using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class StorageIdResponse
    {
        [JsonProperty("orderId")]
        public int orderId { get; set; }
        [JsonProperty("offchainId")]
        public int offchainId { get; set; }
    }
}
