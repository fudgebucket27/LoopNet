using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The wallet data info
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Is it in counterfactual status
        /// </summary>
        [JsonProperty("isInCounterFactualStatus")]
        public bool IsInCounterFactualStatus { get; set; }
        /// <summary>
        /// Is it a contract?
        /// </summary>
        [JsonProperty("isContract")]
        public bool IsContract { get; set; }
        /// <summary>
        /// The contract version
        /// </summary>
        [JsonProperty("loopringWalletContractVersion")]
        public string? LoopringWalletContractVersion { get; set; }
    }
    /// <summary>
    /// The wallet type API response
    /// </summary>
    public class WalletTypeResponse
    {
        /// <summary>
        /// Additional info from Loppring API
        /// </summary>
        [JsonProperty("resultInfo")]
        public ResultInfo? ResultInfo { get; set; }
        /// <summary>
        /// The wallet data info
        /// </summary>
        [JsonProperty("data")]
        public Data? Data { get; set; }
    }
    /// <summary>
    /// Additional info from Loopring API
    /// </summary>
    public class ResultInfo
    {
        /// <summary>
        /// The code
        /// </summary>
        [JsonProperty("resultInfo")]
        public int Code { get; set; }
        /// <summary>
        /// The message
        /// </summary>
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
