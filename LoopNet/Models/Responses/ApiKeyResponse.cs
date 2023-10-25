using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The API Key API response
    /// </summary>
    public class ApiKeyResponse
    {
        /// <summary>
        /// The api key
        /// </summary>
        [JsonProperty("apiKey")]
        public string? ApiKey { get; set; }
    }
}
