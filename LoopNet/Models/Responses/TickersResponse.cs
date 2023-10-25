using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The tickers API response
    /// </summary>
    public class TickersResponse
    {
        /// <summary>
        /// The tickers
        /// </summary>
        [JsonProperty("tickers")]
        public List<List<string>>? Tickers { get; set; }
    }
}
