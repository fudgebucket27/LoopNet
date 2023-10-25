namespace LoopNet.Models.Requests
{
    /// <summary>
    /// The order type
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// AMM
        /// </summary>
        AMM,
        /// <summary>
        /// Limit order
        /// </summary>
        LIMIT_ORDER,
        /// <summary>
        /// Maker only
        /// </summary>
        MAKER_ONLY,
        /// <summary>
        /// Taker only
        /// </summary>
        TAKER_ONLY
    }
}
