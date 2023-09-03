using LoopNet.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Responses
{
    public class Pending
    {
        [JsonProperty("withdraw")]
        public string? Withdraw { get; set; }

        [JsonProperty("deposit")]
        public string? Deposit { get; set; }

    }

    public class NftBalanceResponse
    {
        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }

        [JsonProperty("data")]
        public List<Datum>? Data { get; set; }
    }

    public class CoinBalance
    {
        [JsonProperty("accountId")]
        public int AccountId { get; set; }

        [JsonProperty("tokenId")]
        public int TokenId { get; set; }

        [JsonProperty("total")]
        public string? Total { get; set; }

        [JsonProperty("locked")]
        public string? Locked { get; set; }

        [JsonProperty("pending")]
        public Pending? Pending { get; set; }
    }

    public class NftBase
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("image")]
        public string? Image { get; set; }

        [JsonProperty("properties")]
        public string? Properties { get; set; }

        [JsonProperty("localization")]
        public string? Localization { get; set; }

        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }
    }

    public class Cached
    {
        [JsonProperty("avatar")]
        public string? Avatar { get; set; }

        [JsonProperty("banner")]
        public string? Banner { get; set; }

        [JsonProperty("tileUri")]
        public string? TileUri { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
    }

    public class CollectionInfo
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

        [JsonProperty("baseUri")]
        public string? BaseUri { get; set; }

        [JsonProperty("nftFactory")]
        public string? NftFactory { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("avatar")]
        public string? Avatar { get; set; }

        [JsonProperty("banner")]
        public string? Banner { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonProperty("tileUri")]
        public string? TileUri { get; set; }

        [JsonProperty("cached")]
        public Cached? Cached { get; set; }

        [JsonProperty("deployStatus")]
        public string? DeployStatus { get; set; }

        [JsonProperty("nftType")]
        public string? NftType { get; set; }

        [JsonProperty("times")]
        public Times? Times { get; set; }

        [JsonProperty("extra")]
        public Extra? Extra { get; set; }
    }

    public class Datum
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("accountId")]
        public int AccountId { get; set; }

        [JsonProperty("tokenId")]
        public int TokenId { get; set; }

        [JsonProperty("nftData")]
        public string? NftData { get; set; }

        [JsonProperty("tokenAddress")]
        public string? TokenAddress { get; set; }

        [JsonProperty("nftId")]
        public string? NftId { get; set; }

        [JsonProperty("nftType")]
        public string? NftType { get; set; }

        [JsonProperty("total")]
        public string? Total { get; set; }

        [JsonProperty("locked")]
        public string? Locked { get; set; }

        [JsonProperty("pending")]
        public Pending? Pending { get; set; }

        [JsonProperty("deploymentStatus")]
        public string? DeploymentStatus { get; set; }

        [JsonProperty("isCounterFactualNFT")]
        public bool IsCounterFactualNFT { get; set; }

        [JsonProperty("metadata")]
        public Metadata? Metadata { get; set; }

        [JsonProperty("minter")]
        public string? Minter { get; set; }

        [JsonProperty("royaltyPercentage")]
        public int? RoyaltyPercentage { get; set; }

        [JsonProperty("preference")]
        public Preference? Preference { get; set; }

        [JsonProperty("collectionInfo")]
        public CollectionInfo? CollectionInfo { get; set; }

        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }

        [JsonProperty("balanceUpdatedAt")]
        public long BalanceUpdatedAt { get; set; }
    }

    public class Extra
    {
        [JsonProperty("imageData")]
        public string? ImageData { get; set; }

        [JsonProperty("externalUrl")]
        public string? ExternalUrl { get; set; }

        [JsonProperty("attributes")]
        public string? Attributes { get; set; }

        [JsonProperty("backgroundColor")]
        public string? BackgroundColor { get; set; }

        [JsonProperty("animationUrl")]
        public string? AnimationUrl { get; set; }

        [JsonProperty("youtubeUrl")]
        public string? YoutubeUrl { get; set; }

        [JsonProperty("minter")]
        public string? Minter { get; set; }

        [JsonProperty("properties")]
        public Properties? Properties { get; set; }

        [JsonProperty("mintChannel")]
        public string? MintChannel { get; set; }
    }

    public class ImageSize
    {
        [JsonProperty("240-240")]
        public string? Size240 { get; set; }

        [JsonProperty("332-332")]
        public string? Size332 { get; set; }
        [JsonProperty("original")]
        public string? Original { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("uri")]
        public string? Uri { get; set; }

        [JsonProperty("base")]
        public NftBase? NftBase { get; set; }

        [JsonProperty("imageSize")]
        public ImageSize? ImageSize { get; set; }

        [JsonProperty("extra")]
        public Extra? Extra { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("nftType")]
        public int NftType { get; set; }

        [JsonProperty("network")]
        public int Network { get; set; }

        [JsonProperty("tokenAddress")]
        public string? TokenAddress { get; set; }

        [JsonProperty("nftId")]
        public string? NftId { get; set; }
    }

    public class Preference
    {
        [JsonProperty("favourite")]
        public bool Favourite { get; set; }

        [JsonProperty("hide")]
        public bool Hide { get; set; }
    }

    public class Properties
    {
        [JsonProperty("isLegacy")]
        public bool IsLegacy { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("isCounterFactualNFT")]
        public bool IsCounterFactualNFT { get; set; }

        [JsonProperty("isMintable")]
        public bool IsMintable { get; set; }

        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty("isDeletable")]
        public bool IsDeletable { get; set; }
    }

    public class Times
    {
        [JsonProperty("createdAt")]
        public long CreatedAt { get; set; }
        [JsonProperty("updatedAt")]
        public long UpdatedAt { get; set; }
    }

}
