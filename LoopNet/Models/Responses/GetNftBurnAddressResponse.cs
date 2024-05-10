using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// Burn address info from Loopring
    /// </summary>
    public class GetNftBurnAddressResponse
    {
        /// <summary>
        /// Additional info from Loppring API
        /// </summary>
        [JsonProperty("resultInfo")]
        public BurnAddressResultInfo? ResultInfo { get; set; }
        /// <summary>
        /// The result
        /// </summary>
        [JsonProperty("result")]
        public string? Result { get; set; }
    }
    
    /// <summary>
    /// Additional info from Loopring API
    /// </summary>
    public class BurnAddressResultInfo
    {
        /// <summary>
        /// The code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// The message
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }
    }

}
