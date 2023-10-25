namespace LoopNet.Models.Requests
{
    /// <summary>
    /// The counterfactual nft info
    /// </summary>
    public class CounterFactualNftInfo
    {
        /// <summary>
        /// The nft owner
        /// </summary>
        public string? NftOwner { get; set; }
        /// <summary>
        /// The nft factory
        /// </summary>
        public string? NftFactory { get; set; }
        /// <summary>
        /// The nft base uri
        /// </summary>
        public string? NftBaseUri { get; set; } = "";
    }
}