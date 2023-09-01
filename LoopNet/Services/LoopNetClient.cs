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

namespace LoopNet.Services
{
    public class LoopNetClient : ILoopNetClient, IDisposable
    {
        private protected readonly RestClient _loopNetClient;
        private protected string? _l1PrivateKey;
        private protected string? _l2PrivateKey;
        private protected string? _ethAddress;
        private protected string? _apiKey;
        private protected AccountInformationResponse? _accountInformation;

        private LoopNetClient(string l1PrivateKey, string ethAddress)
        {
            _loopNetClient = new RestClient("https://api3.loopring.io");
            _l1PrivateKey = l1PrivateKey;
            _ethAddress = ethAddress;
        }

        /// <summary>
        /// Creates an instance of the LoopNet client.
        /// </summary>
        /// <param name="l1PrivateKey">The L1 Private Key</param>
        /// <param name="ethAddress">The Eth address associated with the L1 Private Key in Ox format</param>
        /// <param name="showConnectionInfo">Indicates whether to display the connection info. Optional, defaults to false.</param>
        /// <returns>A LoopNetClient</returns>
        public static async Task<LoopNetClient> CreateLoopNetClientAsync(string l1PrivateKey, string ethAddress, bool showConnectionInfo = false)
        {
            var instance = new LoopNetClient(l1PrivateKey, ethAddress);
            if (showConnectionInfo == true)
                Console.WriteLine("Connecting to Loopring...");
            await instance.GetApiKeyAsync();
            if (showConnectionInfo == true)
                Console.WriteLine("Connected to Loopring...");
            return instance;
        }

        public void Dispose()
        {
            _loopNetClient?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets tickers.
        /// </summary>
        /// <param name="pairs">A comma seperated list of pairs, ie: LRC-ETH,LRC-WBTC</param>
        /// <returns>The tickers</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey one of the pairs do not exist</exception>
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

        /// <summary>
        /// Gets the markets
        /// </summary>
        /// <returns>The markets</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API</exception>
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

        /// <summary>
        /// Gets account information
        /// </summary>
        /// <param name="owner">The address to retrieve account information from</param>
        /// <returns>The account information</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the address does not exist on Loopring</exception>
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

        /// <summary>
        /// Gets the api key and sets it in the LoopNet client
        /// </summary>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be an issue with the L1 Private Key and Address being used</exception>
        private protected async Task GetApiKeyAsync()
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
            var (secretKey, ethAddress, publicKeyX, publicKeyY) = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, _ethAddress, skipPublicKeyCalculation);

            //Generating the x-api-sig header details for the get loopring api key endpoint
            var apiSignatureBase = "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" + accountId;
            var apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
            var eddsa = new Eddsa(apiSignatureBaseBigInteger, secretKey);
            var xApiSig = eddsa.Sign();

            var request = new RestRequest("api/v3/apiKey");
            request.AddHeader("X-API-SIG", xApiSig);
            request.AddParameter("accountId", accountId);
            var response = await _loopNetClient.ExecuteGetAsync<ApiKeyResponse>(request);
            if (response.IsSuccessful)
            {
                _l2PrivateKey = secretKey;
                _apiKey = response.Data!.ApiKey;
                _loopNetClient.AddDefaultHeader("X-API-KEY", _apiKey!);
            }
            else
            {
                throw new Exception($"Error getting api key, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <summary>
        /// Gets storage id
        /// </summary>
        /// <param name="sellTokenId">The sell token id</param>
        /// <returns>The storage id</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the sellTokenId does not exist</exception>
        public async Task<StorageIdResponse?> GetStorageIdAsync(int sellTokenId)
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

        /// <summary>
        /// Gets offchain fee
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="feeToken">The fee token, ie LRC</param>
        /// <param name="amount">The amount</param>
        /// <returns>The offchain fee</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, most likey the fee token or request type doest not exist</exception>
        public async Task<OffchainFeeResponse?> GetOffchainFeeAsync(int requestType, string feeToken, string amount)
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

        /// <summary>
        /// Transfers a token to an address
        /// </summary>
        /// <param name="toAddress">The address to transfer the token to</param>
        /// <param name="transferTokenSymbol">The token symbol to transfer, ie ETH</param>
        /// <param name="tokenAmount">The amount of the transfer token to sell in decimal, ie 0.1m</param>
        /// <param name="feeTokenSymbol">The token symbol to pay fees in, ie LRC</param>
        /// <param name="memo">The memo to send,</param>
        /// <param name="payAccountActivationFee">Whether you want to pay the toAddress account activation fee. Optional, Defaults to false</param>
        /// <returns>The transfer token reponnse</returns>
        /// <exception cref="Exception">Thrown when there is an issue with the Loopring API, could be due to a number of issues such as storageId, offchainFee or not having enough balance of the transfer token</exception>
        public async Task<TransferTokenResponse> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol,  string memo, bool payAccountActivationFee = false)
        {
            int feeTokenId = 0; //Default to 0 for ETH, 1 is LRC
            int transferTokenId = 0; //Default to 0 for ETH, 1 is LRC
            if((transferTokenSymbol != "LRC" && transferTokenSymbol != "ETH") || (feeTokenSymbol != "LRC" && feeTokenSymbol != "ETH") )
            {
                throw new Exception($"LoopNet can only works with LRC or ETH!");
            }
            
            if(transferTokenSymbol == "LRC")
            {
                transferTokenId = 1;
            }

            if(feeTokenSymbol == "LRC")
            {
                feeTokenId = 1;
            }

            var exchangeAddress = "0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4";
            var amount = (tokenAmount * 1000000000000000000m).ToString("0");
            OffchainFeeResponse? offchainFee;
            if (payAccountActivationFee == true)
            {
                offchainFee = await GetOffchainFeeAsync(15, transferTokenSymbol, amount);
            }
            else
            {
                offchainFee = await GetOffchainFeeAsync(3, transferTokenSymbol, amount); 
            }            
            var feeAmount = offchainFee!.Fees!.Where(w => w.Token == feeTokenSymbol).First().Fee;
            var transferStorageId = await GetStorageIdAsync(transferTokenId);

            //Setup transfer token request
            var req = new TransferTokenRequest()
            {
                Exchange = exchangeAddress,
                MaxFee = new Token()
                {
                    TokenId = feeTokenId,
                    Volume = feeAmount
                },
                Token = new Token()
                {
                    TokenId = transferTokenId,
                    Volume = amount
                },
                PayeeAddr = toAddress,
                PayerAddr = _accountInformation!.Owner,
                PayeeId = 0,
                PayerId = _accountInformation!.AccountId,
                StorageId = transferStorageId!.OffchainId,
                ValidUntil = DateTimeOffset.Now.AddDays(365).ToUnixTimeSeconds(),
                TokenName = transferTokenSymbol,
                TokenFeeName = transferTokenSymbol
            };

            //Calculate eddsa signature
            BigInteger[] eddsaSignatureinputs = {
            LoopNetUtils.ParseHexUnsigned(req.Exchange),
            (BigInteger)req.PayerId,
            (BigInteger)req.PayeeId,
            (BigInteger)req.Token.TokenId,
            BigInteger.Parse(req.Token.Volume),
            (BigInteger)req.MaxFee.TokenId,
            BigInteger.Parse(req.MaxFee.Volume!),
            LoopNetUtils.ParseHexUnsigned(req.PayeeAddr),
            0,
            0,
            (BigInteger)req.ValidUntil,
            (BigInteger)req.StorageId
            };

            Poseidon poseidonTransfer = new(13, 6, 53, "poseidon", 5, _securityTarget: 128);
            BigInteger poseidonTransferHash = poseidonTransfer.CalculatePoseidonHash(eddsaSignatureinputs);
            Eddsa eddsaTransfer = new(poseidonTransferHash, _l2PrivateKey);
            string eddsaSignature = eddsaTransfer.Sign();

            //Calculate ecdsa
            string primaryTypeNameTransfer = "Transfer";
            var eip712TypedDataTransfer = new TypedData()
            {
                Domain = new Domain()
                {
                    Name = "Loopring Protocol",
                    Version = "3.6.0",
                    ChainId = 1, //1 for mainnet
                    VerifyingContract = exchangeAddress,
                },
                PrimaryType = primaryTypeNameTransfer,
                Types = new Dictionary<string, MemberDescription[]>()
                {
                    ["EIP712Domain"] = new[]
                {
                    new MemberDescription {Name = "name", Type = "string"},
                    new MemberDescription {Name = "version", Type = "string"},
                    new MemberDescription {Name = "chainId", Type = "uint256"},
                    new MemberDescription {Name = "verifyingContract", Type = "address"},
                },
                    [primaryTypeNameTransfer] = new[]
                {
                    new MemberDescription {Name = "from", Type = "address"},            // payerAddr
                    new MemberDescription {Name = "to", Type = "address"},              // toAddr
                    new MemberDescription {Name = "tokenID", Type = "uint16"},          // token.tokenId 
                    new MemberDescription {Name = "amount", Type = "uint96"},           // token.volume 
                    new MemberDescription {Name = "feeTokenID", Type = "uint16"},       // maxFee.tokenId
                    new MemberDescription {Name = "maxFee", Type = "uint96"},           // maxFee.volume
                    new MemberDescription {Name = "validUntil", Type = "uint32"},       // validUntill
                    new MemberDescription {Name = "storageID", Type = "uint32"}         // storageId
                },

                },
                Message = new[]
            {
                new MemberValue {TypeName = "address", Value = _accountInformation.Owner},
                new MemberValue {TypeName = "address", Value = toAddress},
                new MemberValue {TypeName = "uint16", Value = req.Token.TokenId},
                new MemberValue {TypeName = "uint96", Value = BigInteger.Parse(req.Token.Volume)},
                new MemberValue {TypeName = "uint16", Value = req.MaxFee.TokenId},
                new MemberValue {TypeName = "uint96", Value = BigInteger.Parse(req.MaxFee.Volume!)},
                new MemberValue {TypeName = "uint32", Value = req.ValidUntil},
                new MemberValue {TypeName = "uint32", Value = req.StorageId},
            }
            };

            var signerTransfer = new Eip712TypedDataSigner();
            var ethECKeyTransfer = new EthECKey(_l1PrivateKey!);
            var encodedTypedDataTransfer = signerTransfer.EncodeTypedData(eip712TypedDataTransfer);
            var ECDRSASignatureTransfer = ethECKeyTransfer.SignAndCalculateV(Sha3Keccack.Current.CalculateHash(encodedTypedDataTransfer));
            var serializedECDRSASignatureTransfer = EthECDSASignature.CreateStringSignature(ECDRSASignatureTransfer);
            var ecdsaSignature = serializedECDRSASignatureTransfer + "0" + (int)2;

            var request = new RestRequest("api/v3/transfer");
            request.AddHeader("x-api-key", _apiKey!);
            request.AddHeader("x-api-sig", ecdsaSignature);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("exchange", exchangeAddress);
            request.AddParameter("payerId", _accountInformation!.AccountId);
            request.AddParameter("payerAddr", _accountInformation.Owner);
            request.AddParameter("payeeId", 0);
            request.AddParameter("payeeAddr", toAddress);
            request.AddParameter("token.tokenId", req.Token.TokenId);
            request.AddParameter("token.volume", req.Token.Volume);
            request.AddParameter("maxFee.tokenId", req.MaxFee.TokenId);
            request.AddParameter("maxFee.volume", req.MaxFee.Volume);
            request.AddParameter("storageId", req.StorageId);
            request.AddParameter("validUntil", req.ValidUntil);
            request.AddParameter("eddsaSignature", eddsaSignature);
            request.AddParameter("ecdsaSignature", ecdsaSignature);
            request.AddParameter("memo", memo);
            if (payAccountActivationFee == true)
            {
                request.AddParameter("payPayeeUpdateAccount", "true");
            }
            
            var response = await _loopNetClient.ExecutePostAsync<TransferTokenResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data!;
            }
            else
            {
                throw new Exception($"Error posting token transfer, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }
    }
}