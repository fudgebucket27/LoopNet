using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The public key
    /// </summary>
    public class PublicKey
    {
        /// <summary>
        /// Public key, x value
        /// </summary>
        [JsonProperty("x")]
        public string? X { get; set; }
        /// <summary>
        /// Public key, y value
        /// </summary>
        [JsonProperty("y")]
        public string? Y { get; set; }
    }
    /// <summary>
    /// Contains Loopring Account Information
    /// </summary>
    public class AccountInformationResponse
    {
        /// <summary>
        /// Account Id
        /// </summary>
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        /// <summary>
        /// Owner
        /// </summary>
        [JsonProperty("owner")]
        public string? Owner { get; set; }
        /// <summary>
        /// If funds are frozen, usually when transferring between layers
        /// </summary>
        [JsonProperty("frozen")]
        public bool Frozen { get; set; }
        /// <summary>
        /// The public key
        /// </summary>
        [JsonProperty("publicKey")]
        public PublicKey? PublicKey { get; set; }
        /// <summary>
        /// Tags
        /// </summary>
        [JsonProperty("tags")]
        public string? Tags { get; set; }
        /// <summary>
        /// The nonce
        /// </summary>
        [JsonProperty("nonce")]
        public int None { get; set; }
        /// <summary>
        /// The nonce - 1
        /// </summary>
        [JsonProperty("keyNonce")]
        public int KeyNonce { get; set; }
        /// <summary>
        /// The key seed used in personal sign unlocking to retrieve L2 Key details
        /// </summary>
        [JsonProperty("keySeed")]
        public string? KeySeed { get; set; }
    }
}
