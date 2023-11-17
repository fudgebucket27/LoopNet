using LoopNet.Models.Helpers;
using LoopNet.Models.Requests;
using LoopNet.Models.Responses;
using Nethereum.Signer.EIP712;
using Nethereum.Signer;
using Nethereum.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PoseidonSharp;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using JsonFlatten;

namespace LoopNet.Services
{
    public partial class LoopNetClient
    {
        /// <inheritdoc/>
        public async Task<RedPacketMintResponse?> PostMintRedPacketAsync(long validSince, long validUntil, RedPacketType redPacketType, RedPacketViewType redPacketViewType, RedPacketAmountType redPacketAmountType, int tokenToSend, decimal amountPerPacket, decimal amountOfPackets, string memo, string feeTokenSymbol, decimal? giftAmount = null)
        {
            var maxFeeTokenId = 0;
            if (feeTokenSymbol != "LRC" && feeTokenSymbol != "ETH")
            {
                throw new Exception("LoopNet only works with LRC or ETH!");
            }

            if (feeTokenSymbol == "LRC")
            {
                maxFeeTokenId = 1;
            }

            var validUntil30Days = validUntil + (30 * 86400);

            var offchainFee = await GetOffChainFeeWithAmountAsync(0, 24, 1);
            var storageId = await GetStorageIdAsync(tokenToSend);
            bool isBlind = false;
            if (redPacketType == RedPacketType.Blind_Box)
            {
                isBlind = true;
            }

            //Calculate eddsa signautre
            BigInteger[] poseidonInputs =
        {
                                    LoopNetUtils.ParseHexUnsigned(_chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress),
                                    (BigInteger) _accountInformation!.AccountId,
                                    (BigInteger) 0,
                                    (BigInteger) tokenToSend,
                                    isBlind ? BigInteger.Parse(amountPerPacket.ToString("0")) * BigInteger.Parse(1000000000000000000m.ToString("0")) : BigInteger.Parse(amountPerPacket.ToString("0")) * BigInteger.Parse(amountOfPackets.ToString("0")) * BigInteger.Parse(1000000000000000000m.ToString("0")),
                                    (BigInteger) maxFeeTokenId,
                                    BigInteger.Parse(offchainFee!.Fees![maxFeeTokenId].Fee!),
                                    LoopNetUtils.ParseHexUnsigned(_chainId == 1 ? "0x9cde4366824d9410fb2e2f885601933a926f40d7" : "0xa3961aae9522f0f66f2406ac6faa2af0a8bfe504"),
                                    (BigInteger) 0,
                                    (BigInteger) 0,
                                    (BigInteger) validUntil30Days,
                                    (BigInteger) storageId!.OffchainId
                    };
            Poseidon poseidon = new Poseidon(13, 6, 53, "poseidon", 5, _securityTarget: 128);
            BigInteger poseidonHash = poseidon.CalculatePoseidonHash(poseidonInputs);
            Eddsa eddsa = new Eddsa(poseidonHash, _l2PrivateKey);
            string eddsaSignature = eddsa.Sign();

            //Calculate ecdsa
            string primaryTypeName = "Transfer";
            TypedData eip712TypedData = new TypedData();
            eip712TypedData.Domain = new Domain()
            {
                Name = "Loopring Protocol",
                Version = "3.6.0",
                ChainId = _chainId == 1 ? 1 : 5,
                VerifyingContract = _chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress,
            };
            eip712TypedData.PrimaryType = primaryTypeName;
            eip712TypedData.Types = new Dictionary<string, MemberDescription[]>()
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

            };
            eip712TypedData.Message = new[]
            {
                                    new MemberValue {TypeName = "address", Value = _accountInformation.Owner},
                                    new MemberValue {TypeName = "address", Value = _chainId == 1 ? "0x9cde4366824d9410fb2e2f885601933a926f40d7" : "0xa3961aae9522f0f66f2406ac6faa2af0a8bfe504"},
                                    new MemberValue {TypeName = "uint16", Value = tokenToSend},
                                    new MemberValue {TypeName = "uint96", Value = isBlind ? BigInteger.Parse(amountPerPacket.ToString("0")) * BigInteger.Parse(1000000000000000000m.ToString("0")) : BigInteger.Parse(amountPerPacket.ToString("0")) * BigInteger.Parse(amountOfPackets.ToString("0")) * BigInteger.Parse(1000000000000000000m.ToString("0"))},
                                    new MemberValue {TypeName = "uint16", Value = maxFeeTokenId},
                                    new MemberValue {TypeName = "uint96", Value = BigInteger.Parse(offchainFee.Fees[maxFeeTokenId].Fee!)},
                                    new MemberValue {TypeName = "uint32", Value = validUntil30Days},
                                    new MemberValue {TypeName = "uint32", Value = storageId.OffchainId},
                                };

            Eip712TypedDataSigner signer = new Eip712TypedDataSigner();
            var ethECKey = new Nethereum.Signer.EthECKey(_counterFactualWalletInfo == null ? _l1PrivateKey! : _l2PrivateKey!.Replace("0x", ""));
            var encodedTypedData = signer.EncodeTypedData(eip712TypedData);
            var ECDRSASignature = ethECKey.SignAndCalculateV(Sha3Keccack.Current.CalculateHash(encodedTypedData));
            var serializedECDRSASignature = EthECDSASignature.CreateStringSignature(ECDRSASignature);
            var ecdsaSignature = serializedECDRSASignature + "0" + (int)2;

            //Red packet details
            RedPacket redPacketNft = new RedPacket();
            redPacketNft.EcdsaSignature = ecdsaSignature;
            LuckyToken luckyToken = new LuckyToken();
            luckyToken.Exchange = _chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress;
            luckyToken.PayerAddr = _accountInformation.Owner;
            luckyToken.PayerId = _accountInformation.AccountId;
            luckyToken.PayeeAddr = _chainId == 1 ? "0x9cde4366824d9410fb2e2f885601933a926f40d7" : "0xa3961aae9522f0f66f2406ac6faa2af0a8bfe504";
            luckyToken.StorageId = storageId.OffchainId;
            luckyToken.Token = tokenToSend;

            //Red packet details
            if (redPacketType == RedPacketType.Blind_Box)
            {
                luckyToken.Amount = (amountPerPacket * 1000000000000000000m).ToString("0");
                redPacketNft.GiftNumbers = (giftAmount!.Value * 1000000000000000000m).ToString("0");
            }
            else
            {
                luckyToken.Amount = (amountPerPacket * amountOfPackets * 1000000000000000000m).ToString("0");
            }
            luckyToken.FeeToken = maxFeeTokenId;
            luckyToken.MaxFeeAmount = offchainFee.Fees[maxFeeTokenId].Fee;
            luckyToken.ValidUntil = validUntil30Days;
            luckyToken.PayeeId = 0;
            luckyToken.Memo = $"LuckTokenSendBy{_accountInformation.AccountId}";
            luckyToken.EddsaSig = eddsaSignature;
            redPacketNft.LuckyToken = luckyToken;
            redPacketNft.Memo = memo;
            redPacketNft.Numbers = amountOfPackets.ToString("0");
            redPacketNft.SignerFlag = false;
            redPacketNft.TemplateId = 0;

            redPacketNft.Type = new LoopNet.Models.Requests.Type()
            {
                Partition = (int)redPacketAmountType,
                Scope = (int)redPacketViewType,
                Mode = (int)redPacketType
            };

            redPacketNft.ValidSince = validSince;
            redPacketNft.ValidUntil = validUntil;

            var request = new RestRequest(LoopNetConstantsHelper.PostRedPacketEndpoint);
            request.AddHeader("x-api-key", _apiKey!);
            request.AddHeader("x-api-sig", ecdsaSignature);
            request.AddHeader("Accept", "application/json");
            var jObject = JObject.Parse(JsonConvert.SerializeObject(redPacketNft));
            var jObjectFlattened = jObject.Flatten();
            var jObjectFlattenedString = JsonConvert.SerializeObject(jObjectFlattened);
            request.AddParameter("application/json", jObjectFlattenedString, ParameterType.RequestBody);
            var response = await _loopNetClient!.ExecutePostAsync<RedPacketMintResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting red packet mint, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }

        /// <inheritdoc/>
        public async Task<OrderResponse?> PostOrderAsync(Token sellToken, Token buyToken, bool allOrNone, bool fillAmountBOrS, int validUntil, int maxFeeBips = 20, string? clientOrderId = null, OrderType? orderType = null, TradeChannel? tradeChannel = null, string? taker = null, string? poolAddress = null, string? affiliate = null)
        {
            var order = new Order()
            {
                Exchange = _chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress,
                AccountId = _accountInformation!.AccountId,
                StorageId = (await GetStorageIdAsync(sellToken.TokenId))!.OrderId, // MAYBE? NOT SURE
                SellToken = sellToken,
                BuyToken = buyToken,
                AllOrNone = allOrNone,
                FillAmountBOrS = fillAmountBOrS,
                ValidUntil = validUntil,
                MaxFeeBips = maxFeeBips,
            };
            if (!string.IsNullOrWhiteSpace(clientOrderId))
                order.ClientOrderId = clientOrderId;
            if (orderType.HasValue)
                order.OrderType = orderType.Value.ToString();
            if (tradeChannel.HasValue)
                order.TradeChannel = tradeChannel.Value.ToString();
            if (!string.IsNullOrWhiteSpace(taker))
                order.Taker = taker;
            if (!string.IsNullOrWhiteSpace(poolAddress))
                order.PoolAddress = poolAddress;
            if (!string.IsNullOrWhiteSpace(affiliate))
                order.Affiliate = affiliate;


            BigInteger[] inputs = {
                LoopNetUtils.ParseHexUnsigned(order.Exchange),
                order.StorageId,
                order.AccountId,
                order.SellToken.TokenId,
                order.BuyToken.TokenId,
                BigInteger.Parse(order.SellToken.Volume!),
                BigInteger.Parse(order.BuyToken.Volume!),
                order.ValidUntil,
                order.MaxFeeBips,
                (fillAmountBOrS ? 1 : 0),
                string.IsNullOrWhiteSpace(order.Taker) ? 0 : LoopNetUtils.ParseHexUnsigned(order.Taker)
            };

            var poseidonHasher = new Poseidon(inputs.Length + 1, 6, 53, "poseidon", 5, _securityTarget: 128);
            var poseidonHash = poseidonHasher.CalculatePoseidonHash(inputs);
            var poseidonEddsa = new Eddsa(poseidonHash, _l2PrivateKey);
            var eddsaSignature = poseidonEddsa.Sign();
            order.EddsaSignature = eddsaSignature;
            string serializedOrder = JsonConvert.SerializeObject(order);

            var request = new RestRequest(LoopNetConstantsHelper.PostOrderApiEndpoint);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", serializedOrder, ParameterType.RequestBody);

            var response = await _loopNetClient!.ExecutePostAsync<OrderResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting order, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
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

            var exchangeAddress = _chainId == 1 ? LoopNetConstantsHelper.ProductionExchangeAddress : LoopNetConstantsHelper.TestExchangeAddress;
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
                    ChainId = _chainId == 1 ? 1 : 5, //1 for mainnet
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

            var response = await _loopNetClient!.ExecutePostAsync<TransferTokenResponse>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception($"Error posting token transfer, HTTP Status Code:{response.StatusCode}, Content:{response.Content}");
            }
        }



    }
}
