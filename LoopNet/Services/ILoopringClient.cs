using LoopNet.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Services
{
    public interface ILoopringClient
    {
        Task<TickersResponse?> GetTickersAsync(string pairs);
        Task<MarketsResponse?> GetMarketsAsync();
        Task<AccountInformationResponse?> GetAccountInformationAsync(string owner);
        Task GetApiKeyAsync();
    }
}
