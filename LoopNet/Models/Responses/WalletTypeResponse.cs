using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class Data
    {
        [JsonProperty("isInCounterFactualStatus")]
        public bool IsInCounterFactualStatus { get; set; }

        [JsonProperty("isContract")]
        public bool IsContract { get; set; }

        [JsonProperty("loopringWalletContractVersion")]
        public string? LoopringWalletContractVersion { get; set; }
    }

    public class WalletTypeResponse
    {
        [JsonProperty("resultInfo")]
        public ResultInfo? ResultInfo { get; set; }

        [JsonProperty("data")]
        public Data? Data { get; set; }
    }

    public class ResultInfo
    {
        [JsonProperty("resultInfo")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}
