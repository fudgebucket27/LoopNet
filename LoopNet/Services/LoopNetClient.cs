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

namespace LoopNet.Services
{
    /// <summary>
    /// The LoopNet Client class
    /// </summary>
    public class LoopNetClient : ILoopNetClient, IDisposable
    {
        private protected readonly RestClient _loopNetClient;
        private protected string? _l1PrivateKey;
        private protected string? _l2PrivateKey;
        private protected string? _ethAddress;
        private protected string? _apiKey;
        private protected AccountInformationResponse? _accountInformation;
        private protected CounterFactualWalletInfoResponse? _counterFactualWalletInfo;

        private LoopNetClient(string l1PrivateKey, string ethAddress)
        {
            _loopNetClient = new RestClient(LoopNetConstantsHelper.ProductionLoopringApiEndpoint);
            _l1PrivateKey = l1PrivateKey;
            _ethAddress = ethAddress;
        }

        /// <summary>
        /// Creates an instance of the LoopNet client. This will also generate the Loopring L2 Private Key and retrieve the Loopring API Key.
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

        /// <inheritdoc />
        public async Task<WalletTypeResponse?> GetWalletTypeAsync(string walletAddress)
        {
            var request = new RestRequest("api/wallet/v3/wallet/type");
            //request.AddHeader("x-api-key", apiKey);
            request.AddParameter("wallet", walletAddress);
            var response = await _loopNetClient.ExecuteGetAsync<WalletTypeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting tickers, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc />
        public async Task<TickersResponse?> GetTickersAsync(string pairs)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetTickersApiEndpoint);
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

        /// <inheritdoc/>
        public async Task<MarketsResponse?> GetMarketsAsync()
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetMarketsApiEndpoint);
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

        /// <inheritdoc/>
        public async Task<AccountInformationResponse?> GetAccountInformationAsync(string owner)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetAccountInformationApiEndpoint);
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
            var walletType = await GetWalletTypeAsync(_ethAddress!);
            //Getting the account information
            _accountInformation = await GetAccountInformationAsync(_ethAddress!);
            _counterFactualWalletInfo = await GetWalletCounterFactualInfoAsync(_accountInformation!.AccountId);
            if (string.IsNullOrEmpty(_accountInformation?.KeySeed))
            {
                _accountInformation!.KeySeed = $"Sign this message to access Loopring Exchange: 0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4 with key nonce: {_accountInformation.KeyNonce - 1}";
            }
            var messageToSign = _accountInformation?.KeySeed;
            int accountId = _accountInformation!.AccountId;

            var skipPublicKeyCalculation = false; //set to false to generate the public key details as well, set to true to skip public key generation which makes it run faster

            var signer = new EthereumMessageSigner();
            var signedMessageECDSA = signer.EncodeUTF8AndSign(messageToSign, new EthECKey(_l1PrivateKey));
            if (walletType!.Data!.IsContract == true || walletType!.Data!.IsInCounterFactualStatus== true)
            {
                signedMessageECDSA += "02";
            }
            var (secretKey, ethAddress, publicKeyX, publicKeyY) = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, _ethAddress, skipPublicKeyCalculation);

            //Generating the x-api-sig header details for the get loopring api key endpoint
            var apiSignatureBase = "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" + accountId;
            var apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
            var eddsa = new Eddsa(apiSignatureBaseBigInteger, secretKey);
            var xApiSig = eddsa.Sign();

            var request = new RestRequest(LoopNetConstantsHelper.GetApiKeyApiEndpoint);
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

        /// <inheritdoc/>
        public async Task<StorageIdResponse?> GetStorageIdAsync(int sellTokenId)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetStoragIdApiEndpoint);
            request.AddHeader("X-API-KEY", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("sellTokenId", sellTokenId);
            var response = await _loopNetClient.ExecuteGetAsync<StorageIdResponse>(request);
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
            var response = await _loopNetClient.ExecuteGetAsync<OffchainFeeResponse>(request);
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
        public async Task<TransferTokenResponse?> PostTokenTransferAsync(string toAddress, string transferTokenSymbol, decimal tokenAmount, string feeTokenSymbol, string memo, bool payAccountActivationFee = false)
        {
            int feeTokenId = 0; //Default to 0 for ETH, 1 is LRC
            int transferTokenId = 0; //Default to 0 for ETH, 1 is LRC
            if ((transferTokenSymbol != "LRC" && transferTokenSymbol != "ETH") || (feeTokenSymbol != "LRC" && feeTokenSymbol != "ETH"))
            {
                throw new Exception($"LoopNet only works with LRC or ETH!");
            }

            if (transferTokenSymbol == "LRC")
            {
                transferTokenId = 1;
            }

            if (feeTokenSymbol == "LRC")
            {
                feeTokenId = 1;
            }

            var exchangeAddress = LoopNetConstantsHelper.ExchangeAddress;
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
            var ethECKeyTransfer = new EthECKey(_counterFactualWalletInfo == null ? _l1PrivateKey! : _l2PrivateKey!.Replace("0x", ""));
            var encodedTypedDataTransfer = signerTransfer.EncodeTypedData(eip712TypedDataTransfer);
            var ECDRSASignatureTransfer = ethECKeyTransfer.SignAndCalculateV(Sha3Keccack.Current.CalculateHash(encodedTypedDataTransfer));
            var serializedECDRSASignatureTransfer = EthECDSASignature.CreateStringSignature(ECDRSASignatureTransfer);
            var ecdsaSignature = serializedECDRSASignatureTransfer + "0" + (int)2;

            var request = new RestRequest(LoopNetConstantsHelper.PostTokenTransferApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddHeader("x-api-sig", ecdsaSignature );
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
            if(_counterFactualWalletInfo == null)
            {
                request.AddParameter("ecdsaSignature", ecdsaSignature);
            }
            else
            {
                request.AddParameter("counterFactualInfo.accountId", _accountInformation.AccountId);
                request.AddParameter("counterFactualInfo.wallet", _counterFactualWalletInfo!.Wallet);
                request.AddParameter("counterFactualInfo.walletFactory", _counterFactualWalletInfo.WalletFactory);
                request.AddParameter("counterFactualInfo.walletSalt", _counterFactualWalletInfo.WalletSalt);
                request.AddParameter("counterFactualInfo.walletOwner", _counterFactualWalletInfo.WalletOwner);
            }
            request.AddParameter("memo", memo);
            if (payAccountActivationFee == true)
            {
                request.AddParameter("payPayeeUpdateAccount", "true");
            }

            var response = await _loopNetClient.ExecutePostAsync<TransferTokenResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting token transfer, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<PostNftMintResponse?> PostLegacyMintNft(string ipfsMetadataJsonCidv0, int numberOfEditions, int royaltyPercentage, string tokenFeeSymbol, string? royaltyAddress = null)
        {
            var counterFactualNftInfo = new CounterFactualNftInfo
            {
                NftOwner = _accountInformation!.Owner,
                NftFactory = LoopNetConstantsHelper.LegacyNftFactoryContract,
                NftBaseUri = ""
            };
            var feeTokenId = 0;
            if (tokenFeeSymbol != "LRC" && tokenFeeSymbol != "ETH")
            {
                throw new Exception("LoopNet only works with LRC or ETH!");
            }

            if (tokenFeeSymbol == "LRC")
            {
                feeTokenId = 1;
            }

            var counterFactualNft = await GetCounterFactualNftTokenAddressAsync(counterFactualNftInfo);
            var offchainFee = await GetOffchainFeeNftAsync(9, counterFactualNft!.TokenAddress!);
            var storageId = await GetStorageIdAsync(feeTokenId);

            var ipfsCidv0Regex = new Regex(@"^Qm[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz]{44}$");
            if (!ipfsCidv0Regex.IsMatch(ipfsMetadataJsonCidv0))
            {
                throw new Exception("IPFS metadata json cid is not in v0 format, ie starts with Qm!");
            }

            var multiHash = Multihash.Parse(ipfsMetadataJsonCidv0, Multiformats.Base.MultibaseEncoding.Base58Btc);
            var multiHashString = multiHash.ToString();
            var ipfsCidBigInteger = LoopNetUtils.ParseHexUnsigned(multiHashString);
            var nftId = "0x" + ipfsCidBigInteger.ToString("x").Substring(4);

            //Generate the poseidon hash for the nft data
            var nftIdHi = LoopNetUtils.ParseHexUnsigned(nftId.Substring(0, 34));
            var nftIdLo = LoopNetUtils.ParseHexUnsigned(nftId.Substring(34, 32));
            var nftDataPoseidonInputs = new BigInteger[]
            {
                LoopNetUtils.ParseHexUnsigned(_accountInformation.Owner!),
                (BigInteger) 0,
                LoopNetUtils.ParseHexUnsigned(counterFactualNft.TokenAddress!),
                nftIdLo,
                nftIdHi,
                (BigInteger)royaltyPercentage
            };
            var nftDataPoseidon = new Poseidon(7, 6, 52, "poseidon", 5, _securityTarget: 128);
            var nftDataPoseidonHash = nftDataPoseidon.CalculatePoseidonHash(nftDataPoseidonInputs);

            //Generate the poseidon hash for the remaining data
            var validUntil = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds();
            var nftPoseidonInputs = new BigInteger[]
            {
                LoopNetUtils.ParseHexUnsigned(LoopNetConstantsHelper.ExchangeAddress),
                (BigInteger) _accountInformation.AccountId,
                (BigInteger) _accountInformation.AccountId,
                nftDataPoseidonHash,
                (BigInteger) numberOfEditions,
                (BigInteger) feeTokenId,
                BigInteger.Parse(offchainFee!.Fees![feeTokenId].Fee!),
                (BigInteger) validUntil,
                (BigInteger) storageId!.OffchainId
            };
            var nftPoseidon = new Poseidon(10, 6, 53, "poseidon", 5, _securityTarget: 128);
            var nftPoseidonHash = nftPoseidon.CalculatePoseidonHash(nftPoseidonInputs);

            //Generate the poseidon eddsa signature
            Eddsa eddsa = new Eddsa(nftPoseidonHash, _l2PrivateKey);
            string eddsaSignature = eddsa.Sign();

            var request = new RestRequest(LoopNetConstantsHelper.PostNftMintApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("exchange", LoopNetConstantsHelper.ExchangeAddress);
            request.AddParameter("minterId", _accountInformation.AccountId);
            request.AddParameter("minterAddress", _accountInformation.Owner);
            request.AddParameter("toAccountId", _accountInformation.AccountId);
            request.AddParameter("toAddress", _accountInformation.Owner);
            request.AddParameter("nftType", 0); //0 is ERC-1155, 1 is ERC-721 but Loopring does not support that yet...
            request.AddParameter("tokenAddress", counterFactualNft.TokenAddress);
            request.AddParameter("nftId", nftId);
            request.AddParameter("amount", numberOfEditions);
            request.AddParameter("validUntil", validUntil);
            request.AddParameter("royaltyPercentage", royaltyPercentage);
            request.AddParameter("storageId", storageId.OffchainId);
            request.AddParameter("maxFee.tokenId", feeTokenId);
            request.AddParameter("maxFee.amount", offchainFee.Fees[feeTokenId].Fee);
            request.AddParameter("forceToMint", "false");
            if (!string.IsNullOrEmpty(royaltyAddress))
            {
                request.AddParameter("royaltyAddress", royaltyAddress);
            }
            else
            {
                request.AddParameter("royaltyAddress", _accountInformation.Owner);
            }
            request.AddParameter("counterFactualNftInfo.nftFactory", counterFactualNftInfo.NftFactory);
            request.AddParameter("counterFactualNftInfo.nftOwner", counterFactualNftInfo.NftOwner);
            request.AddParameter("counterFactualNftInfo.nftBaseUri", counterFactualNftInfo.NftBaseUri);
            request.AddParameter("eddsaSignature", eddsaSignature);
            var response = await _loopNetClient.ExecutePostAsync<PostNftMintResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting legacy nft mint, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<OffchainFeeResponse?> GetOffchainFeeNftAsync(int requestType, string tokenAddress)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetOffchainFeeNftApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("requestType", requestType);
            request.AddParameter("tokenAddress", tokenAddress);
            var response = await _loopNetClient.ExecuteGetAsync<OffchainFeeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting offchain fee for nft mint, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<OffchainFeeResponse?> GetOffchainFeeNftTransferAsync(int requestType, string amount)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetOffchainFeeNftApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("requestType", requestType);
            request.AddParameter("amount", amount);
            var response = await _loopNetClient.ExecuteGetAsync<OffchainFeeResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting offchain fee for nft mint, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<CounterFactualNft?> GetCounterFactualNftTokenAddressAsync(CounterFactualNftInfo counterFactualNftInfo)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetCounterFactualNftTokenAddressApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("nftFactory", counterFactualNftInfo.NftFactory);
            request.AddParameter("nftOwner", counterFactualNftInfo.NftOwner);
            request.AddParameter("nftBaseUri", counterFactualNftInfo.NftBaseUri);

            var response = await _loopNetClient.ExecuteGetAsync<CounterFactualNft>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting counterfactual nft token address, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<PostNftMintResponse?> PostNftMintAsync(string contractAddress, string ipfsMetadataJsonCidv0, int numberOfEditions, int royaltyPercentage, string tokenFeeSymbol, string? royaltyAddress = null)
        {
            var feeTokenId = 0;
            if (tokenFeeSymbol != "LRC" && tokenFeeSymbol != "ETH")
            {
                throw new Exception("LoopNet only works with LRC or ETH!");
            }

            if (tokenFeeSymbol == "LRC")
            {
                feeTokenId = 1;
            }

            var nftCollectionInfo = await GetNftCollectionInfoAsync(contractAddress);
            if (nftCollectionInfo!.Collections!.Count == 0)
            {
                throw new Exception("That contract does not exist under your address!");
            }

            //Getting the token address
            CounterFactualNftInfo counterFactualNftInfo = new CounterFactualNftInfo
            {
                NftOwner = _accountInformation!.Owner,
                NftFactory = LoopNetConstantsHelper.CurrentNftFactoryContract,
                NftBaseUri = nftCollectionInfo.Collections[0].Collection!.BaseUri
            };

            var counterFactualNft = await GetCounterFactualNftTokenAddressAsync(counterFactualNftInfo);
            var offchainFee = await GetOffchainFeeNftAsync(9, counterFactualNft!.TokenAddress!);
            var storageId = await GetStorageIdAsync(feeTokenId);

            var ipfsCidv0Regex = new Regex(@"^Qm[123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz]{44}$");
            if (!ipfsCidv0Regex.IsMatch(ipfsMetadataJsonCidv0))
            {
                throw new Exception("IPFS metadata json cid is not in v0 format, ie starts with Qm!");
            }

            var multiHash = Multihash.Parse(ipfsMetadataJsonCidv0, Multiformats.Base.MultibaseEncoding.Base58Btc);
            var multiHashString = multiHash.ToString();
            var ipfsCidBigInteger = LoopNetUtils.ParseHexUnsigned(multiHashString);
            var nftId = "0x" + ipfsCidBigInteger.ToString("x").Substring(4);

            //Generate the poseidon hash for the nft data
            var nftIdHi = LoopNetUtils.ParseHexUnsigned(nftId.Substring(0, 34));
            var nftIdLo = LoopNetUtils.ParseHexUnsigned(nftId.Substring(34, 32));
            var nftDataPoseidonInputs = new BigInteger[]
            {
                LoopNetUtils.ParseHexUnsigned(_accountInformation.Owner!),
                (BigInteger) 0,
                LoopNetUtils.ParseHexUnsigned(counterFactualNft.TokenAddress!),
                nftIdLo,
                nftIdHi,
                (BigInteger)royaltyPercentage
            };
            var nftDataPoseidon = new Poseidon(7, 6, 52, "poseidon", 5, _securityTarget: 128);
            var nftDataPoseidonHash = nftDataPoseidon.CalculatePoseidonHash(nftDataPoseidonInputs);

            //Generate the poseidon hash for the remaining data
            var validUntil = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds();
            var nftPoseidonInputs = new BigInteger[]
            {
                LoopNetUtils.ParseHexUnsigned(LoopNetConstantsHelper.ExchangeAddress),
                (BigInteger) _accountInformation.AccountId,
                (BigInteger) _accountInformation.AccountId,
                nftDataPoseidonHash,
                (BigInteger) numberOfEditions,
                (BigInteger) feeTokenId,
                BigInteger.Parse(offchainFee!.Fees![feeTokenId].Fee!),
                (BigInteger) validUntil,
                (BigInteger) storageId!.OffchainId
            };
            var nftPoseidon = new Poseidon(10, 6, 53, "poseidon", 5, _securityTarget: 128);
            var nftPoseidonHash = nftPoseidon.CalculatePoseidonHash(nftPoseidonInputs);

            //Generate the poseidon eddsa signature
            Eddsa eddsa = new Eddsa(nftPoseidonHash, _l2PrivateKey);
            string eddsaSignature = eddsa.Sign();

            var request = new RestRequest(LoopNetConstantsHelper.PostNftMintApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("exchange", LoopNetConstantsHelper.ExchangeAddress);
            request.AddParameter("minterId", _accountInformation.AccountId);
            request.AddParameter("minterAddress", _accountInformation.Owner);
            request.AddParameter("toAccountId", _accountInformation.AccountId);
            request.AddParameter("toAddress", _accountInformation.Owner);
            request.AddParameter("nftType", 0); //0 is ERC-1155, 1 is ERC-721 but Loopring does not support that yet...
            request.AddParameter("tokenAddress", counterFactualNft.TokenAddress);
            request.AddParameter("nftId", nftId);
            request.AddParameter("amount", numberOfEditions);
            request.AddParameter("validUntil", validUntil);
            request.AddParameter("royaltyPercentage", royaltyPercentage);
            request.AddParameter("storageId", storageId.OffchainId);
            request.AddParameter("maxFee.tokenId", feeTokenId);
            request.AddParameter("maxFee.amount", offchainFee.Fees[feeTokenId].Fee);
            request.AddParameter("forceToMint", "false");
            if (!string.IsNullOrEmpty(royaltyAddress))
            {
                request.AddParameter("royaltyAddress", royaltyAddress);
            }
            else
            {
                request.AddParameter("royaltyAddress", _accountInformation.Owner);
            }
            request.AddParameter("counterFactualNftInfo.nftFactory", counterFactualNftInfo.NftFactory);
            request.AddParameter("counterFactualNftInfo.nftOwner", counterFactualNftInfo.NftOwner);
            request.AddParameter("counterFactualNftInfo.nftBaseUri", counterFactualNftInfo.NftBaseUri);
            request.AddParameter("eddsaSignature", eddsaSignature);
            var response = await _loopNetClient.ExecutePostAsync<PostNftMintResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting legacy nft mint, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<GetNftCollectionInfoResponse?> GetNftCollectionInfoAsync(string tokenAddress)
        {
            var request = new RestRequest("api/v3/nft/collection");
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("limit", 12);
            request.AddParameter("offset", 0);
            request.AddParameter("owner", _accountInformation!.Owner);
            request.AddParameter("tokenAddress", tokenAddress);
            var response = await _loopNetClient.ExecuteGetAsync<GetNftCollectionInfoResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting nft collection info, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<NftBalanceResponse?> GetNftTokenIdAsync(string nftData)
        {
            var request = new RestRequest("/api/v3/user/nft/balances");
            request.AddHeader("x-api-key", _apiKey!);
            request.AddParameter("accountId", _accountInformation!.AccountId);
            request.AddParameter("nftDatas", nftData);
            request.AddParameter("metadata", "true");
            var response = await _loopNetClient.ExecuteGetAsync<NftBalanceResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error getting nft token Id, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<TransferTokenResponse?> PostNftTransferAsync(string toAddress, string nftData, int amountOfEditionsToTransfer, string feeTokenSymbol, string memo, bool payAccountActivationFee = false)
        {
            int feeTokenId = 0; //Default to 0 for ETH, 1 is LRC
            if (feeTokenSymbol != "LRC" && feeTokenSymbol != "ETH")
            {
                throw new Exception($"LoopNet only works with LRC or ETH!");
            }

            if (feeTokenSymbol == "LRC")
            {
                feeTokenId = 1;
            }

            var nftInfo = await GetNftTokenIdAsync(nftData);
            if(nftInfo!.TotalNum == 0)
            {
                throw new Exception("Can not find token id for the given nftData!");
            }
            var nftTokenId = nftInfo!.Data![0].TokenId;

            OffchainFeeResponse? offchainFee;
            if (payAccountActivationFee == true)
            {
                offchainFee = await GetOffchainFeeNftTransferAsync(11, "0");
            }
            else
            {
                offchainFee = await GetOffchainFeeNftTransferAsync(19, "0");
            }
            var feeAmount = offchainFee!.Fees!.Where(w => w.Token == feeTokenSymbol).First().Fee;
            var storageId = await GetStorageIdAsync(nftTokenId);

            //Calculate eddsa signautre
            var validUntil = DateTimeOffset.Now.AddDays(30).ToUnixTimeSeconds();
            var poseidonInputs = new BigInteger[]
            {
                            LoopNetUtils.ParseHexUnsigned(LoopNetConstantsHelper.ExchangeAddress),
                            (BigInteger) _accountInformation!.AccountId,
                            (BigInteger) 0,
                            (BigInteger) nftTokenId,
                            BigInteger.Parse(amountOfEditionsToTransfer.ToString()),
                            (BigInteger) feeTokenId,
                            BigInteger.Parse(offchainFee.Fees![feeTokenId].Fee!),
                            LoopNetUtils.ParseHexUnsigned(toAddress),
                            (BigInteger) 0,
                            (BigInteger) 0,
                            (BigInteger) validUntil,
                            (BigInteger) storageId!.OffchainId
            };
            Poseidon poseidon = new(13, 6, 53, "poseidon", 5, _securityTarget: 128);
            BigInteger poseidonHash = poseidon.CalculatePoseidonHash(poseidonInputs);
            Eddsa eddsa = new(poseidonHash, _l2PrivateKey);
            string eddsaSignature = eddsa.Sign();

            //Calculate ecdsa
            string primaryTypeName = "Transfer";
            TypedData eip712TypedData = new()
            {
                Domain = new Domain()
                {
                    Name = "Loopring Protocol",
                    Version = "3.6.0",
                    ChainId = 1,
                    VerifyingContract = LoopNetConstantsHelper.ExchangeAddress,
                },
                PrimaryType = primaryTypeName,
                Types = new Dictionary<string, MemberDescription[]>()
                {
                    ["EIP712Domain"] = new[]
                    {
                                    new MemberDescription {Name = "name", Type = "string"},
                                    new MemberDescription {Name = "version", Type = "string"},
                                    new MemberDescription {Name = "chainId", Type = "uint256"},
                                    new MemberDescription {Name = "verifyingContract", Type = "address"},
                                },
                    [primaryTypeName] = new[]
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
                            new MemberValue {TypeName = "uint16", Value = nftTokenId},
                            new MemberValue {TypeName = "uint96", Value = BigInteger.Parse(amountOfEditionsToTransfer.ToString())},
                            new MemberValue {TypeName = "uint16", Value = feeTokenId},
                            new MemberValue {TypeName = "uint96", Value = BigInteger.Parse(offchainFee.Fees[feeTokenId].Fee!)},
                            new MemberValue {TypeName = "uint32", Value = validUntil},
                            new MemberValue {TypeName = "uint32", Value = storageId.OffchainId},
                        }
            };


            var signerTransfer = new Eip712TypedDataSigner();
            var ethECKeyTransfer = new EthECKey(_counterFactualWalletInfo == null ? _l1PrivateKey! : _l2PrivateKey!.Replace("0x", ""));
            var encodedTypedDataTransfer = signerTransfer.EncodeTypedData(eip712TypedData);
            var ECDRSASignatureTransfer = ethECKeyTransfer.SignAndCalculateV(Sha3Keccack.Current.CalculateHash(encodedTypedDataTransfer));
            var serializedECDRSASignatureTransfer = EthECDSASignature.CreateStringSignature(ECDRSASignatureTransfer);
            var ecdsaSignature = serializedECDRSASignatureTransfer + "0" + (int)2;

            var request = new RestRequest(LoopNetConstantsHelper.PostNftTransferApiEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddHeader("x-api-sig", ecdsaSignature);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("exchange", LoopNetConstantsHelper.ExchangeAddress);
            request.AddParameter("fromAccountId", _accountInformation.AccountId);
            request.AddParameter("fromAddress", _accountInformation.Owner);
            request.AddParameter("toAccountId", 0);
            request.AddParameter("toAddress", toAddress);
            request.AddParameter("token.tokenId", nftTokenId);
            request.AddParameter("token.amount", amountOfEditionsToTransfer);
            request.AddParameter("token.nftData", nftData);
            request.AddParameter("maxFee.tokenId", feeTokenId);
            request.AddParameter("maxFee.amount", offchainFee.Fees[feeTokenId].Fee);
            request.AddParameter("storageId", storageId.OffchainId);
            request.AddParameter("validUntil", validUntil);
            if (_counterFactualWalletInfo == null)
            {
                request.AddParameter("ecdsaSignature", ecdsaSignature);
            }
            else
            {
                request.AddParameter("counterFactualInfo.accountId", _accountInformation.AccountId);
                request.AddParameter("counterFactualInfo.wallet", _counterFactualWalletInfo!.Wallet);
                request.AddParameter("counterFactualInfo.walletFactory", _counterFactualWalletInfo.WalletFactory);
                request.AddParameter("counterFactualInfo.walletSalt", _counterFactualWalletInfo.WalletSalt);
                request.AddParameter("counterFactualInfo.walletOwner", _counterFactualWalletInfo.WalletOwner);
            }
            request.AddParameter("memo", memo);
            if (payAccountActivationFee == true)
            {
                request.AddParameter("payPayeeUpdateAccount", "true");
            }
            var response = await _loopNetClient.ExecutePostAsync<TransferTokenResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting nft transfer, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<CounterFactualWalletInfoResponse?> GetWalletCounterFactualInfoAsync(int accountId)
        {
            var request = new RestRequest(LoopNetConstantsHelper.GetWalletCounterfactualInfoApiEndpoint);
            request.AddParameter("accountId", accountId);
            var response = await _loopNetClient.ExecuteGetAsync<CounterFactualWalletInfoResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                return null; //response will not be successful if counterfactual info can not be found so just return null;
            }
        }
    }
}