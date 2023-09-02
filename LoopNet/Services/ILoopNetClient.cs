using LoopNet.Models.Requests;
using LoopNet.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Services
{
    public interface ILoopNetClient
    {
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
        /// Transfers a token to an address
        /// </summary>
        /// <param name="toAddress">The address to transfer the token to</param>
        /// <param name="transferTokenSymbol">The token symbol to transfer, Only works with ETH or LRC</param>
        /// <param name="tokenAmount">The amount of the transfer token to sell in decimals, ie 0.1m</param>
        /// <param name="feeTokenSymbol">The token symbol to pay fees in, ie Only works with ETH or LRC</param>
        /// <param name="memo">The memo to send,</param>
        /// <param name="payAccountActivationFee">Whether you want to pay the toAddress account activation fee. Optional, Defaults to false</param>
        /// <returns>The transfer token reponnse</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be due to a number of issues such as storageId, offchainFee or not having enough balance of the transfer token</exception>
        Task<TransferTokenResponse?> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol, string memo, bool payAccountActivationFee = false);

        /// <summary>
        /// Get the counterfactual nft token address
        /// </summary>
        /// <param name="counterFactualNftInfo">The Counterfactual NFT info</param>
        /// <returns>The counterfactual nft token address</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<CounterFactualNft?> GetCounterFactualNftTokenAddressAsync(CounterFactualNftInfo counterFactualNftInfo);

        /// <summary>
        /// Post the nft mint to the legacy nft factor contract
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
        /// Gets the offchain fee for an nft request
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="tokenAddress">The token address</param>
        /// <returns>The offchain fee for an nft request</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<OffchainFeeResponse?> GetOffchainFeeNftAsync(int requestType, string tokenAddress);

        /// <summary>
        /// Gets information on an nft collection
        /// </summary>
        /// <param name="tokenAddress">The token address</param>
        /// <returns>The nft collection result</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
        Task<GetNftCollectionInfoResponse?> GetNftCollectionInfoAsync(string tokenAddress);
    }
}
