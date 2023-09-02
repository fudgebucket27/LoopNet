using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    public class CounterFactualNft
    {
        [JsonProperty("tokenAddress")]
        public string? TokenAddress { get; set; }
    }
}
