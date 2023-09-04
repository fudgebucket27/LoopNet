using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class NftHolder
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("tokenId")]
        public int TokenId { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class NftHoldersResponse
    {
        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }

        [JsonProperty("nftHolders")]
        public List<NftHolder>? NftHolders { get; set; }
    }
}
