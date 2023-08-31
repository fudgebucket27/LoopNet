using RestSharp;
using System.Diagnostics;
using LoopNet.Models.Responses;

namespace LoopNet.Services
{
    public class LoopringClient : ILoopringClient
    {
        readonly RestClient _client;

        public LoopringClient()
        {
            _client = new RestClient("https://api3.loopring.io");
        }
        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<TickersResponse?> GetTickersAsync(string pairs)
        {
            var request = new RestRequest("api/v3/ticker");
            request.AddParameter("market", pairs);
            try
            {
                var response = await _client.GetAsync<TickersResponse>(request);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<MarketsResponse?> GetMarketsAsync()
        {
            var request = new RestRequest("api/v3/exchange/markets");
            try
            {
                var response = await _client.GetAsync<MarketsResponse>(request);
                return response;
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return null;
            }    
        }

        public async Task<AccountInformationResponse?> GetAccountInformationAsync(string owner)
        {
            var request = new RestRequest("api/v3/account");
            request.AddParameter("owner", owner);
            try
            {
                var response = await _client.GetAsync<AccountInformationResponse>(request);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ApiKeyResponse?> GetApiKeyAsync(string xApiSig, int accountId)
        {
            var request = new RestRequest("api/v3/apiKey");
            request.AddHeader("x-api-sig", xApiSig);
            request.AddParameter("accountId", accountId);

            try
            {
                var response = await _client.GetAsync<ApiKeyResponse>(request);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}