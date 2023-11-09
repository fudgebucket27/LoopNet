using LoopNet.Models.Requests;
using LoopNet.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Services
{
    /// <summary>
    /// The LoopNet Client interface
    /// </summary>
    public interface ILoopNetClient
    {
        /// <summary>
        /// Gets the nft balance of a wallet
        /// </summary>
        /// <param name="acccountId">The account id of the wallet. Optional, defaults to own wallet</param>
        /// <returns>A dictionary with keys being nftData and value being the nft balance info details</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, if accountId does not exist</exception>
        Task<Dictionary<string, Datum>?> GetNftWalletBalanceAsync(int? acccountId = null);


        /// <summary>
        /// Gets the nft holders for a given nftData
        /// </summary>
        /// <param name="nftData">The nftData</param>
        /// <returns>A list of holders for the given nftData</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<List<NftHolder>?> GetNftHoldersAsync(string nftData);

        /// <summary>
        /// Gets the wallet type
        /// </summary>
        /// <param name="walletAddress">The wallet address in 0x format.</param>
        /// <returns>The wallet type</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<WalletTypeResponse?> GetWalletTypeAsync(string walletAddress);


        /// <summary>
        /// Gets the counterfactual info for a wallet
        /// </summary>
        /// <remarks>This method returns null if the wallet counterfactual info can not be found</remarks>
        /// <param name="accountId">The account id of the wallet</param>
        /// <returns>The wallet type</returns>
        Task<CounterFactualWalletInfoResponse?> GetWalletCounterFactualInfoAsync(int accountId);

        /// <summary>
        /// Gets tickers.
        /// </summary>
        /// <param name="pairs">A comma seperated list of pairs, ie: LRC-ETH,LRC-WBTC</param>
        /// <returns>The tickers</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey one of the pairs do not exist</exception>
        Task<TickersResponse?> GetTickersAsync(string pairs);

        /// <summary>
        /// Gets the markets
        /// </summary>
        /// <returns>The markets</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<MarketsResponse?> GetMarketsAsync();

        /// <summary>
        /// Gets the exchange tokens
        /// </summary>
        /// <returns>The exchange tokens</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<List<ExchangeTokenResponse>?> GetExchangeTokensAsync();

        /// <summary>
        /// Gets the order user rate amounts
        /// </summary>
        /// <param name="market">The market, ie LRC-ETH</param>
        /// <returns>The order user rate amounts</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<OrderUserRateAmountResponse?> GetOrderUserRateAmountAsync(string market);

        /// <summary>
        /// Gets account information
        /// </summary>
        /// <param name="owner">The address to retrieve account information from</param>
        /// <returns>The account information</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the address does not exist on Loopring</exception>
        Task<AccountInformationResponse?> GetAccountInformationAsync(string owner);

        /// <summary>
        /// Gets storage id
        /// </summary>
        /// <param name="sellTokenId">The sell token id</param>
        /// <returns>The storage id</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the sellTokenId does not exist</exception>
        Task<StorageIdResponse?> GetStorageIdAsync(int sellTokenId);

        /// <summary>
        /// Gets offchain fee
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="feeToken">The fee token, ie LRC</param>
        /// <param name="amount">The amount</param>
        /// <returns>The offchain fee</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the fee token or request type doest not exist</exception>
        Task<OffchainFeeResponse?> GetOffchainFeeAsync(int requestType, string feeToken, string amount);

        /// <summary>
        /// Get the counterfactual nft token address
        /// </summary>
        /// <param name="counterFactualNftInfo">The Counterfactual NFT info</param>
        /// <returns>The counterfactual nft token address</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<CounterFactualNft?> GetCounterFactualNftTokenAddressAsync(CounterFactualNftInfo counterFactualNftInfo);

        /// <summary>
        /// Gets the offchain fee for an nft request
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="tokenAddress">The token address</param>
        /// <returns>The offchain fee for an nft request</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<OffchainFeeResponse?> GetOffchainFeeNftAsync(int requestType, string tokenAddress);

        /// <summary>
        /// Gets the offchain fee for an nft transfer request
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="amount">The amount</param>
        /// <returns>The offchain fee for an nft transfer request</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<OffchainFeeResponse?> GetOffchainFeeNftTransferAsync(int requestType, string amount);

        /// <summary>
        /// Gets information on an nft collection
        /// </summary>
        /// <param name="tokenAddress">The token address</param>
        /// <returns>The nft collection result</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<GetNftCollectionInfoResponse?> GetNftCollectionInfoAsync(string tokenAddress);

        /// <summary>
        /// Get the NFT balance info
        /// </summary>
        /// <param name="nftData">The nftData in 0x format, ie 0x2a212b36db36d229d3ee5690c7f9fe0099b53d6f05cfb0349060f4c18012a664</param>
        /// <returns>The NFT balance info</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, possibly nftData doesnt exist in your wallet</exception>
        Task<NftBalanceResponse?> GetNftTokenIdAsync(string nftData);

        /// <summary>
        /// Transfers a token to an address
        /// </summary>
        /// <param name="toAddress">The address to transfer the token to in 0x format</param>
        /// <param name="transferTokenSymbol">The token symbol to transfer, Only works with ETH or LRC</param>
        /// <param name="tokenAmount">The amount of the transfer token to sell in decimals, ie 0.1m</param>
        /// <param name="feeTokenSymbol">The token symbol to pay fees in, ie Only works with ETH or LRC</param>
        /// <param name="memo">The memo to send,</param>
        /// <param name="payAccountActivationFee">Whether you want to pay the toAddress account activation fee. Optional, Defaults to false</param>
        /// <returns>The transfer token reponnse</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be due to a number of issues such as storageId, offchainFee or not having enough balance of the transfer token</exception>
        Task<TransferTokenResponse?> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol, string memo, bool payAccountActivationFee = false);

        /// <summary>
        /// Transfers a nft to an address
        /// </summary>
        /// <param name="toAddress">The address to transfer the NFT to in 0x format</param>
        /// <param name="nftData">The nftData in 0x format</param>
        /// <param name="amountOfEditionsToTransfer">The amount of editions to transfer</param>
        /// <param name="feeTokenSymbol">The token symbol to pay fees in, ie Only works with ETH or LRC</param>
        /// <param name="memo">The memo to send,</param>
        /// <param name="payAccountActivationFee">Whether you want to pay the toAddress account activation fee. Optional, Defaults to false</param>
        /// <returns>The transfer token reponnse</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be due to a number of issues such as storageId, offchainFee or not having enough balance of the transfer token</exception>
        Task<TransferTokenResponse?> PostNftTransferAsync(string toAddress, string nftData, int amountOfEditionsToTransfer, string feeTokenSymbol, string memo, bool payAccountActivationFee = false);

        /// <summary>
        /// Post the nft mint to the legacy nft factory contract
        /// </summary>
        /// <param name="ipfsMetadataJsonCidv0">The IPFS metadata json in CIDv0 format, ie starts with Qm</param>
        /// <param name="numberOfEditions">The number of editions, Set to 1 for 1 edition, 2 for 2 editions and etc</param>
        /// <param name="royaltyPercentage">The royalty percantage, a whole number between 0 to 10</param>
        /// <param name="tokenFeeSymbol">The token symbol for the fees, can be LRC or ETH</param>
        /// <param name="royaltyAddress">The royalty address in 0x format, Optional. Only set if you want royalties go a different address than yourself</param>
        /// <returns>The Nft Mint Response</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, such as duplicate nft mints</exception>
        Task<PostNftMintResponse?> PostLegacyMintNft(string ipfsMetadataJsonCidv0, int numberOfEditions, int royaltyPercentage, string tokenFeeSymbol, string? royaltyAddress = null);

        /// <summary>
        /// Post the nft mint to the current nft factory contract
        /// </summary>
        /// <param name="contractAddress">The contract address of the collection in 0x format</param>
        /// <param name="ipfsMetadataJsonCidv0">The IPFS metadata json in CIDv0 format, ie starts with Qm</param>
        /// <param name="numberOfEditions">The number of editions, Set to 1 for 1 edition, 2 for 2 editions and etc</param>
        /// <param name="royaltyPercentage">The royalty percantage, a whole number between 0 to 10</param>
        /// <param name="tokenFeeSymbol">The token symbol for the fees, can be LRC or ETH</param>
        /// <param name="royaltyAddress">The royalty address in 0x format, Optional. Only set if you want royalties go a different address than yourself</param>
        /// <returns>The Nft Mint Response</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, such as duplicate nft mints</exception>
        Task<PostNftMintResponse?> PostNftMintAsync(string contractAddress, string ipfsMetadataJsonCidv0, int numberOfEditions, int royaltyPercentage, string tokenFeeSymbol, string? royaltyAddress = null);

        /// <summary>
        /// Post an order to exchange two currencies
        /// </summary>
        /// <param name="sellToken">The token you are selling</param>
        /// <param name="buyToken">The token you are buying</param>
        /// <param name="allOrNone">Whether the order supports partial fills or not.Currently only supports false as a valid value</param>
        /// <param name="fillAmountBOrS">Fill size by buy token or by sell token</param>
        /// <param name="validUntil">Order expiration time, accuracy is in seconds</param>
        /// <param name="maxFeeBips">Maximum order fee that the user can accept, value range (in ten thousandths) 1 ~ 63</param>
        /// <param name="clientOrderId">An arbitrary, client-set unique order identifier, max length is 120 bytes</param>
        /// <param name="orderType">Order types, can be AMM, LIMIT_ORDER, MAKER_ONLY, TAKER_ONLY</param>
        /// <param name="tradeChannel">	Order channel, can be BLANK,ORDER_BOOK, AMM_POOL, MIXED</param>
        /// <param name="taker">Used by the P2P order which user specify the taker, so far its 0x0000000000000000000000000000000000000000</param>
        /// <param name="poolAddress">The AMM pool address if order type is AMM</param>
        /// <param name="affiliate">An accountID who will recieve a share of the fee of this order</param>
        /// <returns>The order response</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, such as an invalid order</exception>
        Task<OrderResponse?> PostOrderAsync(
            Token sellToken,
        Token buyToken,
        bool allOrNone,
        bool fillAmountBOrS,
        int validUntil,
        int maxFeeBips = 20,
        string? clientOrderId = null,
        OrderType? orderType = null,
        TradeChannel? tradeChannel = null,
        string? taker = null,
        string? poolAddress = null,
        string? affiliate = null
        );

        /// <summary>
        /// Post nft mint red packet
        /// </summary>
        /// <param name="validSince">The date when the red packet can be started to claim. Unix Timestamp in seconds in UTC</param>
        /// <param name="validUntil">The date when the red packet claim ends. Unix Timestamp in seconds in UTC</param>
        /// <param name="nftRedPacketType">The nft red packet type</param>
        /// <param name="nftRedPacketViewType">The nft red packet view type public, private or exclusive</param>
        /// <param name="nftRedPacketAmountType">The nft red packet amount distribution type.</param>
        /// <param name="nftData">The nft data</param>
        /// <param name="amountOfNftsPerPacket">Amount of nfts per packets</param>
        /// <param name="amountOfPackets">Amount of packets</param>
        /// <param name="memo">The memo to include</param>
        /// <param name="feeTokenSymbol">The fee token symbol</param>
        /// <param name="giftAmount">Amount to gift if blind box nft, optional</param>
        /// <returns>The red packet mint response</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<RedPacketNftMintResponse?> PostNftMintRedPacket(long validSince, long validUntil, NftRedPacketType nftRedPacketType, NftRedPacketViewType nftRedPacketViewType, NftRedPacketAmountType nftRedPacketAmountType, string nftData, string amountOfNftsPerPacket, string amountOfPackets, string memo, string feeTokenSymbol, string? giftAmount = null);

        /// <summary>
        /// Get nft offchain fee with amount
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <param name="requestType">The request type</param>
        /// <param name="tokenAddress">The token address</param>
        /// <returns>The nft offchain fee with amount</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<OffchainFeeResponse?> GetNftOffChainFeeWithAmount(int amount, int requestType, string tokenAddress);
    }
}
