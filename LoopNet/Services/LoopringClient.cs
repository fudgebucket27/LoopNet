using RestSharp;
using LoopNet.Models;

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

        public async Task<Markets?> GetMarkets()
        {
            var request = new RestRequest("api/v3/exchange/markets");
            var response = await _client.GetAsync<Markets>(request);
            return response;
        }

        public async Task<Tickers?> GetTickers(string pairs)
        {
            var request = new RestRequest("api/v3/ticker");
            request.AddParameter("market", pairs);
            var response = await _client.GetAsync<Tickers>(request);
            return response;
        }
    }
}