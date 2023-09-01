using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class OffchainFee
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
        [JsonProperty("Fee")]
        public string? Fee { get; set; }
        [JsonProperty("Discount")]
        public decimal Discount { get; set; }
    }

    public class OffchainFeeResponse
    {
        [JsonProperty("gasPrice")]
        public string? GasPrice { get; set; }
        [JsonProperty("fees")]
        public List<OffchainFee>? Fees { get; set; }
    }
}
