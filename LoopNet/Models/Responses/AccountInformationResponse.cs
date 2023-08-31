using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class PublicKey
    {
        [JsonProperty("x")]
        public string? X { get; set; }
        [JsonProperty("y")]
        public string? Y { get; set; }
    }

    public class AccountInformationResponse
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        [JsonProperty("owner")]
        public string? Owner { get; set; }
        [JsonProperty("frozen")]
        public bool Frozen { get; set; }
        [JsonProperty("publicKey")]
        public PublicKey? PublicKey { get; set; }
        [JsonProperty("tags")]
        public string? Tags { get; set; }
        [JsonProperty("nonce")]
        public int None { get; set; }
        [JsonProperty("keyNonce")]
        public int KeyNonce { get; set; }
        [JsonProperty("keySeed")]
        public string? KeySeed { get; set; }
    }
}
