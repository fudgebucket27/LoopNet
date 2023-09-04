using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class CounterFactualWalletInfoResponse
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }

        [JsonProperty("wallet")]
        public string? Wallet { get; set; }

        [JsonProperty("walletFactory")]
        public string? WalletFactory { get; set; }

        [JsonProperty("walletSalt")]
        public string? WalletSalt { get; set; }

        [JsonProperty("walletOwner")]
        public string? WalletOwner { get; set; }
    }
}
