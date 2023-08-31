using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class ApiKeyResponse
    {
        [JsonProperty("apiKey")]
        public string? ApiKey { get; set; }
    }
}
