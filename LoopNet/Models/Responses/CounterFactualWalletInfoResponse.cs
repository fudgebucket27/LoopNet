using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    /// <summary>
    /// The counterfactual wallet info API response
    /// </summary>
    public class CounterFactualWalletInfoResponse
    {
        /// <summary>
        /// The account id
        /// </summary>
        [JsonProperty("accountId")]
        public int AccountId { get; set; }
        /// <summary>
        /// The wallet
        /// </summary>
        [JsonProperty("wallet")]
        public string? Wallet { get; set; }
        /// <summary>
        /// The wallet factory
        /// </summary>
        [JsonProperty("walletFactory")]
        public string? WalletFactory { get; set; }
        /// <summary>
        /// The wallet salt
        /// </summary>
        [JsonProperty("walletSalt")]
        public string? WalletSalt { get; set; }
        /// <summary>
        /// The wallet owner
        /// </summary>
        [JsonProperty("walletOwner")]
        public string? WalletOwner { get; set; }
    }
}
