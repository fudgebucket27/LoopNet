using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    /// <summary>
    /// The countefactual nft
    /// </summary>
    public class CounterFactualNft
    {
        /// <summary>
        /// The token address
        /// </summary>
        [JsonProperty("tokenAddress")]
        public string? TokenAddress { get; set; }
    }
}
