using RestSharp;
using System.Diagnostics;
using LoopNet.Models.Responses;
using Nethereum.Signer;
using PoseidonSharp;
using System.Numerics;
using System.Net;
using Nethereum.Signer.Crypto;
using Newtonsoft.Json.Linq;
using LoopNet.Models.Requests;
using LoopNet.Models.Helpers;
using System;
using Nethereum.Signer.EIP712;
using Nethereum.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using Nethereum.Model;
using System.Text.RegularExpressions;
using Multiformats.Hash;
using Nethereum.ABI;
using Org.BouncyCastle.Utilities.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonFlatten;

namespace LoopNet.Services
{
    /// <summary>
    /// The LoopNet Client class
    /// </summary>
    public partial class LoopNetClient : ILoopNetClient, IDisposable
    {
        private protected readonly RestClient? _loopNetClient;
        private protected string? _l1PrivateKey;
        private protected string? _l2PrivateKey;
        private protected string? _ethAddress;
        private protected string? _apiKey;
        private protected int _chainId;
        private protected AccountInformationResponse? _accountInformation;
        private protected CounterFactualWalletInfoResponse? _counterFactualWalletInfo;

        private LoopNetClient(int chainId, string l1PrivateKey, string ethAddress)
        {
            _chainId = chainId;
            if (chainId == 1)
            {
                _loopNetClient = new RestClient(LoopNetConstantsHelper.ProductionLoopringApiEndpoint);
            }
            else if (chainId == 5)
            {
                _loopNetClient = new RestClient(LoopNetConstantsHelper.TestLoopringApiEndpoint);
            }
            _l1PrivateKey = l1PrivateKey;
            _ethAddress = ethAddress;
        }

        /// <summary>
        /// Creates an instance of the LoopNet client. This will also generate the Loopring L2 Private Key and retrieve the Loopring API Key.
        /// </summary>
        /// <param name="chainId">1 for MAINNET, 5 for Test</param>
        /// <param name="l1PrivateKey">The L1 Private Key</param>
        /// <param name="ethAddress">The Eth address associated with the L1 Private Key in Ox format</param>
        /// <param name="showConnectionInfo">Indicates whether to display the connection info. Optional, defaults to false.</param>
        /// <returns>A LoopNetClient</returns>
        public static async Task<LoopNetClient> CreateLoopNetClientAsync(int chainId, string l1PrivateKey, string ethAddress, bool showConnectionInfo = false)
        {
            if (chainId != 1 && chainId != 5)
            {
                throw new Exception("Invalid chainId. 1 for MAINNET, 5 for TEST");
            }
            var instance = new LoopNetClient(chainId, l1PrivateKey, ethAddress);
            if (showConnectionInfo == true)
            {
                Console.WriteLine("Connecting to Loopring...");
            }

            await instance.GetApiKeyAsync();
            if (showConnectionInfo == true)
            {
                Console.WriteLine($"Connected to Loopring... {(chainId == 1 ? "MAINNET" : "TEST")}");
            }
            return instance;
        }

        /// <summary>
        /// Disposes the LoopNet client
        /// </summary>
        public void Dispose()
        {
            _loopNetClient?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public async Task<WalletTypeResponse?> GetWalletTypeAsync(string walletAddress)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetWalletTypeApiEndpoint);
            //request.AddHeader("x-api-key", apiKey);
            request.AddParameter("wallet", walletAddress);
            var response = await _loopNetClient!.ExecuteGetAsync<WalletTypeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting wallet type, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc />
        public async Task<GetNftBurnAddressResponse?> GetNftBurnAddressAsync(int accountId, int tokenId)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetNftBurnAddressApiEndpoint);
            request.AddParameter("accountId", accountId);
            request.AddParameter("tokenId", tokenId);
            var response = await _loopNetClient!.ExecuteGetAsync<GetNftBurnAddressResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting nft burn address, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc />
        public async Task<TickersResponse?> GetTickersAsync(string pairs)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetTickersApiEndpoint);
            request.AddParameter("market", pairs);
            var response = await _loopNetClient!.ExecuteGetAsync<TickersResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting tickers, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<MarketsResponse?> GetMarketsAsync()
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetMarketsApiEndpoint);
            var response = await _loopNetClient!.ExecuteGetAsync<MarketsResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting markets, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<AccountInformationResponse?> GetAccountInformationAsync(string owner)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetAccountInformationApiEndpoint);
            request.AddParameter("owner", owner);
            var response = await _loopNetClient!.ExecuteGetAsync<AccountInformationResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting account information, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <summary>
        /// Gets the api key and sets it in the LoopNet client
        /// </summary>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be an issue with the L1 Private Key and Address being used</exception>
        private protected async Task GetApiKeyAsync()
        {
            var walletType = await GetWalletTypeAsync(_ethAddress!);
            //Getting the account information
            _accountInformation = await GetAccountInformationAsync(_ethAddress!);
            _counterFactualWalletInfo = await GetWalletCounterFactualInfoAsync(_accountInformation!.AccountId);
            if (string.IsNullOrEmpty(_accountInformation?.KeySeed))
            {
                _accountInformation!.KeySeed = $"Sign this message to access Loopring Exchange: {(_chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress)} with key nonce: {_accountInformation.KeyNonce - 1}";
            }
            var messageToSign = _accountInformation?.KeySeed;
            int accountId = _accountInformation!.AccountId;

            var skipPublicKeyCalculation = false; //set to false to generate the public key details as well, set to true to skip public key generation which makes it run faster

            var signer = new EthereumMessageSigner();
            var signedMessageECDSA = signer.EncodeUTF8AndSign(messageToSign, new EthECKey(_l1PrivateKey));
            if (walletType!.Data!.IsContract == true || walletType!.Data!.IsInCounterFactualStatus == true)
            {
                signedMessageECDSA += "02";
            }
            var (secretKey, ethAddress, publicKeyX, publicKeyY) = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, _ethAddress, skipPublicKeyCalculation);

            //Generating the x-api-sig header details for the get loopring api key endpoint
            var apiSignatureEndpoint = _chainId == 1 ? "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" : "GET&https%3A%2F%2Fuat2.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D";
            var apiSignatureBase = apiSignatureEndpoint + accountId;
            var apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
            var eddsa = new Eddsa(apiSignatureBaseBigInteger, secretKey);
            var xApiSig = eddsa.Sign();

            var request = new RestRequest(LoopNetConstantsHelper.GetApiKeyApiEndpoint);
            request.AddHeader("X-API-SIG", xApiSig);
            request.AddParameter("accountId", accountId);
            var response = await _loopNetClient!.ExecuteGetAsync<ApiKeyResponse>(request);
            if (response.IsSuccessful)
            {
                _l2PrivateKey = secretKey;
                _apiKey = response.Data!.ApiKey;
                _loopNetClient!.AddDefaultHeader("X-API-KEY", _apiKey!);
            }
            else
            {
                throw new Exception($"Error getting api key, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<StorageIdResponse?> GetStorageIdAsync(int sellTokenId)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetStoragIdApiEndpoint);
            request.AddHeader("X-API-KEY", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("sellTokenId", sellTokenId);
            var response = await _loopNetClient!.ExecuteGetAsync<StorageIdResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting storage id, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<OffchainFeeResponse?> GetOffchainFeeAsync(int requestType, string feeToken, string amount)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetOffchainFeeApiEndpoint);
            request.AddHeader("X-API-KEY", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("requestType", requestType);
            request.AddParameter("tokenSymbol", feeToken);
            request.AddParameter("amount", amount);
            var response = await _loopNetClient!.ExecuteGetAsync<OffchainFeeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting offchain fee, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<CounterFactualWalletInfoResponse?> GetWalletCounterFactualInfoAsync(int accountId)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetWalletCounterfactualInfoApiEndpoint);
            request.AddParameter("accountId", accountId);
            var response = await _loopNetClient!.ExecuteGetAsync<CounterFactualWalletInfoResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                return null; //response will not be successful if counterfactual info can not be found so just return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ExchangeTokenResponse>?> GetExchangeTokensAsync()
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetExchangeTokensApiEndpoint);
            var response = await _loopNetClient!.ExecuteGetAsync<List<ExchangeTokenResponse>>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting exchange tokens, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<OrderUserRateAmountResponse?> GetOrderUserRateAmountAsync(string market)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetOrderUserRateAmountApiEndpoint);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("market", market);
            var response = await _loopNetClient!.ExecuteGetAsync<OrderUserRateAmountResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting order user rate amount, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<TransferTokenResponse?> PostRedPacketTargetAsync(List<string> addresses, string nftRedPacketHash, int notifyType)
        {
            var redPacketNftTarget = new RedPacketTarget();
            redPacketNftTarget.Claimer = addresses;
            redPacketNftTarget.Hash = nftRedPacketHash;
            redPacketNftTarget.NotifyType = notifyType;

            //Calculate api sig value
            Dictionary<string, object> dataToSig = new Dictionary<string, object>();
            dataToSig.Add("claimer", addresses);
            dataToSig.Add("hash", nftRedPacketHash);
            dataToSig.Add("notifyType", notifyType);
            var signatureBase = "POST&";
            var jObject = JObject.Parse(JsonConvert.SerializeObject(dataToSig));
            var jObjectFlattened = jObject.Flatten();
            var parameterString = JsonConvert.SerializeObject(jObjectFlattened);
            string signatureBaseApiUrl = _chainId == 1 ? LoopNetConstantsHelper.ProductionLoopringApiEndpoint : LoopNetConstantsHelper.TestLoopringApiEndpoint;
            signatureBaseApiUrl += LoopNetConstantsHelper.PostRedPacketTargetApiEndpoint;
            signatureBase += LoopNetUtils.UrlEncodeUpperCase(signatureBaseApiUrl) + "&";
            signatureBase += LoopNetUtils.UrlEncodeUpperCase(parameterString);
            var sha256Number = SHA256Helper.CalculateSHA256HashNumber(signatureBase);
            var sha256Signer = new Eddsa(sha256Number, _l2PrivateKey);
            var xApiSig = sha256Signer.Sign();

            var request = new RestRequest(LoopNetConstantsHelper.PostRedPacketTargetApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddHeader("x-api-sig", xApiSig);
            request.AddHeader("Accept", "application/json");
            var jObjectRequest = JObject.Parse(JsonConvert.SerializeObject(redPacketNftTarget));
            var jObjectFlattenedRequest = jObjectRequest.Flatten();
            var jObjectFlattenedString = JsonConvert.SerializeObject(jObjectFlattenedRequest);
            request.AddParameter("application/json", jObjectFlattenedString, ParameterType.RequestBody);

            var response = await _loopNetClient!.ExecutePostAsync<TransferTokenResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error post red packet target, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }
     
        /// <inheritdoc/>
        public async Task<OffchainFeeResponse?> GetOffChainFeeWithAmountAsync(int amount, int requestType, int extraType)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetOffchainFeeApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("amount", amount);
            request.AddParameter("requestType", requestType);
            request.AddParameter("extraType", extraType);
            var response = await _loopNetClient!.ExecuteGetAsync<OffchainFeeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting nft offchain fee with amount, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }
    }
}