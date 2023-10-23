using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    public class Token
    {
        [JsonProperty("tokenId")]
        public int TokenId { get; set; }
        [JsonProperty("volume")]
        public string? Volume { get; set; }
    }
}
