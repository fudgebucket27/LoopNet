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
    /// <summary>
    /// The nft collection
    /// </summary>
    public class NftCollection
    {
        /// <summary>
        /// The collection id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// The collection owner
        /// </summary>
        [JsonProperty("owner")]
        public string? Owner { get; set; }
        /// <summary>
        /// The collection name
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }
        /// <summary>
        /// The contract address
        /// </summary>
        [JsonProperty("contractAddress")]
        public string? ContractAddress { get; set; }
        /// <summary>
        /// The collection address
        /// </summary>
        [JsonProperty("collectionAddress")]
        public string? CollectionAddress { get; set; }
        /// <summary>
        /// If it's public
        /// </summary>
        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }
        /// <summary>
        /// The base uri
        /// </summary>
        [JsonProperty("baseUri")]
        public string? BaseUri { get; set; }
        /// <summary>
        /// The nft factory
        /// </summary>
        [JsonProperty("nftFactory")]
        public string? NftFactory { get; set; }
        /// <summary>
        /// The collection title
        /// </summary>
        [JsonProperty("collectionTitle")]
        public string? CollectionTitle { get; set; }
        /// <summary>
        /// The collection description
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }
        /// <summary>
        /// The avatar
        /// </summary>
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }
        /// <summary>
        /// The banner
        /// </summary>
        [JsonProperty("banner")]
        public string? Banner { get; set; }
        /// <summary>
        /// The thumbnail
        /// </summary>
        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
        /// <summary>
        /// The cid
        /// </summary>
        [JsonProperty("cid")]
        public string? Cid { get; set; }
        /// <summary>
        /// The tile uri
        /// </summary>
        [JsonProperty("tileUri")]
        public string? TileUri { get; set; }
        /// <summary>
        /// The deploy status to Layer1
        /// </summary>
        [JsonProperty("deployStatus")]
        public string? DeployStatus { get; set; }
        /// <summary>
        /// If it's counterfactual
        /// </summary>
        [JsonProperty("isCounterFactualNFT")]
        public bool IsCounterFactualNFT { get; set; }
        /// <summary>
        /// If it's mintable
        /// </summary>
        [JsonProperty("isMintable")]
        public bool IsMintable { get; set; }
        /// <summary>
        /// The nft type
        /// </summary>
        [JsonProperty("nftType")]
        public string? NftType { get; set; }
        /// <summary>
        /// When it was created
        /// </summary>
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
        /// <summary>
        /// when it was updated
        /// </summary>
        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

    }
    /// <summary>
    /// The collections
    /// </summary>
    public class Collections
    {
        /// <summary>
        /// The collection
        /// </summary>
        [JsonProperty("collection")]
        public NftCollection? Collection { get; set; }

    }

    /// <summary>
    /// The get nft collection info responses
    /// </summary>
    public class GetNftCollectionInfoResponse
    {
        /// <summary>
        /// The collections
        /// </summary>
        [JsonProperty("collections")]
        public List<Collections>? Collections { get; set; }
        /// <summary>
        /// The total number of collections
        /// </summary>
        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }

    }
}
