using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{

    public class NftCollection
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner")]
        public string? Owner { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("contractAddress")]
        public string? ContractAddress { get; set; }
        [JsonProperty("collectionAddress")]
        public string? CollectionAddress { get; set; }
        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
        [JsonProperty("baseUri")]
        public string? BaseUri { get; set; }
        [JsonProperty("nftFactory")]
        public string? NftFactory { get; set; }
        [JsonProperty("collectionTitle")]
        public string? CollectionTitle { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
        [JsonProperty("banner")]
        public string? Banner { get; set; }
        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
        [JsonProperty("cid")]
        public string? Cid { get; set; }
        [JsonProperty("tileUri")]
        public string? TileUri { get; set; }
        [JsonProperty("deployStatus")]
        public string? DeployStatus { get; set; }
        [JsonProperty("isCounterFactualNFT")]
        public bool IsCounterFactualNFT { get; set; }
        [JsonProperty("isMintable")]
        public bool IsMintable { get; set; }
        [JsonProperty("nftType")]
        public string? NftType { get; set; }
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

    }
    public class Collections
    {
        [JsonProperty("collection")]
        public NftCollection? Collection { get; set; }

    }
    public class GetNftCollectionInfoResponse
    {
        [JsonProperty("collections")]
        public List<Collections>? Collections { get; set; }
        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }

    }
}
