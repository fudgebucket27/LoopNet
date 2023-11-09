using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Helpers
{
    /// <summary>
    /// Contains the constants used by LoopNet
    /// </summary>
    public static class LoopNetConstantsHelper
    {
        /// <summary>
        /// The production legacy nft factory contract
        /// </summary>
        public const string ProductionLegacyNftFactoryContract = "0xc852aC7aAe4b0f0a0Deb9e8A391ebA2047d80026";
        /// <summary>
        /// The production current nft factory contract
        /// </summary>
        public const string ProductionCurrentNftFactoryContract = "0x97BE94250AEF1Df307749aFAeD27f9bc8aB911db";
        /// <summary>
        /// The production exchange address
        /// </summary>
        public const string ProductionExchangeAddress = "0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4";
        /// <summary>
        /// The test legacy nft factory contract
        /// </summary>
        public const string TestLegacyNftFactoryContract = "0x355E9941C5e301033ecfD37184E78443c5241035";
        /// <summary>
        /// The test current nft factory contract
        /// </summary>
        public const string TestCurrentNftFactoryContract = "0x0ad87482a1bfd0B3036Bb4b13708C88ACAe1b8bA";
        /// <summary>
        /// The test exchange address
        /// </summary>
        public const string TestExchangeAddress = "0x12b7cccF30ba360e5041C6Ce239C9a188B709b2B";
        /// <summary>
        /// The production loopring api endpoint
        /// </summary>
        public const string ProductionLoopringApiEndpoint = "https://api3.loopring.io";
        /// <summary>
        /// The test loopring api endpoint
        /// </summary>
        public const string TestLoopringApiEndpoint = "https://uat2.loopring.io";
        /// <summary>
        /// The get exchange token configuration api endpoint
        /// </summary>
        public const string GetExchangeTokensApiEndpoint = "api/v3/exchange/tokens";
        /// <summary>
        /// The get tickers api endpoint
        /// </summary>
        public const string GetTickersApiEndpoint = "api/v3/ticker";
        /// <summary>
        /// The get markets api endpoint
        /// </summary>
        public const string GetMarketsApiEndpoint = "api/v3/exchange/markets";
        /// <summary>
        /// The get order user rate amount api endpoint
        /// </summary>
        public const string GetOrderUserRateAmountApiEndpoint = "api/v3/user/orderUserRateAmount";
        /// <summary>
        /// The get account information api endpoint
        /// </summary>
        public const string GetAccountInformationApiEndpoint = "api/v3/account";
        /// <summary>
        /// The get api key api endpoint
        /// </summary>
        public const string GetApiKeyApiEndpoint = "api/v3/apiKey";
        /// <summary>
        /// The get storage id api endpoint
        /// </summary>
        public const string GetStoragIdApiEndpoint = "api/v3/storageId";
        /// <summary>
        /// The get offchain fee api endpoint
        /// </summary>
        public const string GetOffchainFeeApiEndpoint = "api/v3/user/offchainFee";
        /// <summary>
        /// The get offchain fee nft api endpoint
        /// </summary>
        public const string GetOffchainFeeNftApiEndpoint = "api/v3/user/nft/offchainFee";
        /// <summary>
        /// The get counterfactual nft token address api endpoint
        /// </summary>
        public const string GetCounterFactualNftTokenAddressApiEndpoint = "api/v3/nft/info/computeTokenAddress";
        /// <summary>
        /// The get nft collection info api endpoint
        /// </summary>
        public const string GetNftCollectionInfoApiEndpoint = "api/v3/nft/collection";
        /// <summary>
        /// The get wallet counterfactual info api endpoint
        /// </summary>
        public const string GetWalletCounterfactualInfoApiEndpoint = "api/v3/counterFactualInfo";
        /// <summary>
        /// The get nft wallet balance api endpoint
        /// </summary>
        public const string GetNftWalletBalanceApiEndpoint = "/api/v3/user/nft/balances";
        /// <summary>
        /// The get nft holders api endpoint
        /// </summary>
        public const string GetNftHoldersApiEndpoint = "/api/v3/nft/info/nftHolders";
        /// <summary>
        /// The post token transfer api endpoint
        /// </summary>
        public const string PostTokenTransferApiEndpoint = "api/v3/transfer";
        /// <summary>
        /// The post nft mint api endpoint
        /// </summary>
        public const string PostNftMintApiEndpoint = "api/v3/nft/mint";
        /// <summary>
        /// The post nft transfer api endpoint
        /// </summary>
        public const string PostNftTransferApiEndpoint = "api/v3/nft/transfer";
        /// <summary>
        /// The post order api endpoint
        /// </summary>
        public const string PostOrderApiEndpoint = "api/v3/order";

        /// <summary>
        /// The post nft red packet mint api endpoint
        /// </summary>
        public const string PostNftRedPacketMint = "/api/v3/luckyToken/sendLuckyToken";


    }
}
