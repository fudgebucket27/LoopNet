using RestSharp;
using System.Diagnostics;
using LoopNet.Models.Responses;
using Nethereum.Signer;
using PoseidonSharp;
using System.Numerics;
using System.Net;

namespace LoopNet.Services
{
    public class LoopNetClient : ILoopNetClient
    {
        private readonly RestClient _loopNetClient;
        private protected string? _l1PrivateKey;
        private protected string? _ethAddress;
        private protected string? _apiKey;
        private protected AccountInformationResponse? _accountInformation;

        private LoopNetClient(string l1PrivateKey, string ethAddress)
        {
            _loopNetClient = new RestClient("https://api3.loopring.io");
            _l1PrivateKey = l1PrivateKey;
            _ethAddress = ethAddress;
        }

        public static async Task<LoopNetClient> CreateLoopringClientAsync(string l1PrivateKey, string ethAddress)
        {
            var instance = new LoopNetClient(l1PrivateKey, ethAddress);
            await instance.GetApiKeyAsync();
            return instance;
        }

        public void Dispose()
        {
            _loopNetClient?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<TickersResponse?> GetTickersAsync(string pairs)
        {
            var request = new RestRequest("api/v3/ticker");
            request.AddParameter("market", pairs);
            var response = await _loopNetClient.ExecuteGetAsync<TickersResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting tickers, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        public async Task<MarketsResponse?> GetMarketsAsync()
        {
            var request = new RestRequest("api/v3/exchange/markets");
            var response = await _loopNetClient.ExecuteGetAsync<MarketsResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting markets, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        public async Task<AccountInformationResponse?> GetAccountInformationAsync(string owner)
        {
            var request = new RestRequest("api/v3/account");
            request.AddParameter("owner", owner);
            var response = await _loopNetClient.ExecuteGetAsync<AccountInformationResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting account information, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        public async Task GetApiKeyAsync()
        {
            //Getting the account information
            _accountInformation = await GetAccountInformationAsync(_ethAddress!);
            if (string.IsNullOrEmpty(_accountInformation?.KeySeed))
            {
                _accountInformation!.KeySeed = $"Sign this message to access Loopring Exchange: 0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4 with key nonce: {_accountInformation.KeyNonce - 1}";
            }
            var messageToSign = _accountInformation?.KeySeed;
            int accountId = _accountInformation!.AccountId;

            var skipPublicKeyCalculation = false; //set to false to generate the public key details as well, set to true to skip public key generation which makes it run faster

            var signer = new EthereumMessageSigner();
            var signedMessageECDSA = signer.EncodeUTF8AndSign(messageToSign, new EthECKey(_l1PrivateKey));
            var l2KeyDetails = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, _ethAddress, skipPublicKeyCalculation);

            //Generating the x-api-sig header details for the get loopring api key endpoint
            string apiSignatureBase = "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" + accountId;
            BigInteger apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
            Eddsa eddsa = new Eddsa(apiSignatureBaseBigInteger, l2KeyDetails.secretKey);
            var xApiSig = eddsa.Sign();

            var request = new RestRequest("api/v3/apiKey");
            request.AddHeader("X-API-SIG", xApiSig);
            request.AddParameter("accountId", accountId);
            var response = await _loopNetClient.ExecuteGetAsync<ApiKeyResponse>(request);
            if (response.IsSuccessful)
            {
                _apiKey = response.Data!.ApiKey;
                _loopNetClient.AddDefaultHeader("X-API-KEY", _apiKey!);
            }
            else
            {
                throw new Exception($"Error getting api key, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        public async Task<StorageIdResponse?> GetStorageId(int sellTokenId)
        {
            var request = new RestRequest("api/v3/storageId");
            request.AddHeader("X-API-KEY", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("sellTokenId", sellTokenId);
            var response = await _loopNetClient.ExecuteGetAsync<StorageIdResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error getting storage id, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        public async Task<OffchainFeeResponse?> GetOffchainFee(int requestType, string feeToken, string amount)
        {
            var request = new RestRequest("api/v3/user/offchainFee");
            request.AddHeader("X-API-KEY", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("requestType", requestType);
            request.AddParameter("tokenSymbol", feeToken);
            request.AddParameter("amount", amount);
            var response = await _loopNetClient.ExecuteGetAsync<OffchainFeeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error getting offchain fee, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }
    }
}