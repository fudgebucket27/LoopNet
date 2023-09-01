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
        Task<TransferTokenResponse> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol, string memo, bool payAccountActivationFee);
    }
}
