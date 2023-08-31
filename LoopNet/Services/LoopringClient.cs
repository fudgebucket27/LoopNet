using RestSharp;
using System.Diagnostics;
using LoopNet.Models.Responses;

namespace LoopNet.Services
{
    public class LoopringClient
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

        public async Task<Tickers?> GetTickersAsync(string pairs)
        {
            var request = new RestRequest("api/v3/ticker");
            request.AddParameter("market", pairs);
            try
            {
                var response = await _client.GetAsync<Tickers>(request);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Markets?> GetMarketsAsync()
        {
            var request = new RestRequest("api/v3/exchange/markets");
            try
            {
                var response = await _client.GetAsync<Markets>(request);
                return response;
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return null;
            }    
        }

        public async Task<AccountInformation?> GetAccountInformationAsync(string owner)
        {
            var request = new RestRequest("api/v3/account");
            request.AddParameter("owner", owner);
            try
            {
                var response = await _client.GetAsync<AccountInformation>(request);
                return response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<ApiKey?> GetApiKeyAsync(string xApiSig, int accountId)
        {
            var request = new RestRequest("api/v3/apiKey");
            request.AddHeader("x-api-sig", xApiSig);
            request.AddParameter("accountId", accountId);

            try
            {
                var response = await _client.GetAsync<ApiKey>(request);
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