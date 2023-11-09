using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Requests
{
    /// <summary>
    /// The lucky token
    /// </summary>
    public class LuckyToken
    {
        /// <summary>
        /// Exchange
        /// </summary>
        [JsonProperty("exchange")]
        public string? Exchange { get; set; }
        /// <summary>
        /// The payer address
        /// </summary>
        [JsonProperty("payerAddr")]
        public string? PayerAddr { get; set; }
        /// <summary>
        /// The payer id
        /// </summary>
        [JsonProperty("payerId")]
        public int PayerId { get; set; }
        /// <summary>
        /// The payee address
        /// </summary>
        [JsonProperty("payeeAddr")]
        public string? PayeeAddr { get; set; }
        /// <summary>
        /// The storage id
        /// </summary>
        [JsonProperty("storageId")]
        public int StorageId { get; set; }
        /// <summary>
        /// The token
        /// </summary>
        [JsonProperty("token")]
        public int Token { get; set; }
        /// <summary>
        /// The amount
        /// </summary>
        [JsonProperty("amount")]
        public string? Amount { get; set; }
        /// <summary>
        /// The fee token
        /// </summary>
        [JsonProperty("feeToken")]
        public int FeeToken { get; set; }
        /// <summary>
        /// The max fee amount
        /// </summary>
        [JsonProperty("maxFeeAmount")]
        public string? MaxFeeAmount { get; set; }
        /// <summary>
        /// The valid until expiry
        /// </summary>
        [JsonProperty("validUntil")]
        public long ValidUntil { get; set; }
        /// <summary>
        /// The payee id
        /// </summary>
        [JsonProperty("payeeId")]
        public int PayeeId { get; set; }
        /// <summary>
        /// The memo
        /// </summary>
        [JsonProperty("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// The eddsa signature
        /// </summary>
        [JsonProperty("eddsaSig")]
        public string? EddsaSig { get; set; }
    }
    /// <summary>
    /// The Red Packet Nft
    /// </summary>
    public class RedPacketNft
    {
        /// <summary>
        /// The ecdsa signature
        /// </summary>
        [JsonProperty("ecdsaSignature")]
        public string? ecdsaSignature { get; set; }
        /// <summary>
        /// The gift numbers
        /// </summary>
        [JsonProperty("giftNumbers")]
        public string? giftNumbers { get; set; }
        /// <summary>
        /// The lucky token
        /// </summary>
        [JsonProperty("luckyToken")]
        public LuckyToken? LuckyToken { get; set; }
        /// <summary>
        /// The memo
        /// </summary>
        [JsonProperty("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// The nft data
        /// </summary>
        [JsonProperty("nftData")]
        public string? NftData { get; set; }
        /// <summary>
        /// The numbers
        /// </summary>
        [JsonProperty("numbers")]
        public string? Numbers { get; set; }
        /// <summary>
        /// The signer flag
        /// </summary>
        [JsonProperty("signerFlag")]
        public bool SignerFlag { get; set; }
        /// <summary>
        /// The template id
        /// </summary>
        [JsonProperty("templateId")]
        public int TemplateId { get; set; }
        /// <summary>
        /// The type
        /// </summary>
        [JsonProperty("type")]
        public Type? Type { get; set; }
        /// <summary>
        /// The valid since
        /// </summary>
        [JsonProperty("validSince")]
        public long ValidSince { get; set; }
        /// <summary>
        /// The valid until
        /// </summary>
        [JsonProperty("validUntil")]
        public long ValidUntil { get; set; }
    }

    /// <summary>
    /// The type of Red Packet Nft
    /// </summary>
    public class Type
    {
        /// <summary>
        /// The partition
        /// </summary>
        public int partition { get; set; }
        /// <summary>
        /// The mode
        /// </summary>
        public int mode { get; set; }
        /// <summary>
        /// The scope
        /// </summary>
        public int scope { get; set; }
    }

    /// <summary>
    /// The type of Red Packet Nft
    /// </summary>
    public enum NftRedPacketType
    {
        /// <summary>
        /// Normal. Set amount per packet
        /// </summary>
        Normal,
        /// <summary>
        /// blind, chosen at random to recieve
        /// </summary>
        Blind,
        /// <summary>
        /// Random amount is given per packet
        /// </summary>
        Lucky
    }
}
