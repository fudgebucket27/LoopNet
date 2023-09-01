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
        Task<TickersResponse?> GetTickersAsync(string pairs);
        Task<MarketsResponse?> GetMarketsAsync();
        Task<AccountInformationResponse?> GetAccountInformationAsync(string owner);
        Task GetApiKeyAsync();
        Task<StorageIdResponse?> GetStorageIdAsync(int sellTokenId);
        Task<OffchainFeeResponse?> GetOffchainFeeAsync(int requestType, string feeToken, string amount);
        Task<string> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol, string memo);
    }
}
