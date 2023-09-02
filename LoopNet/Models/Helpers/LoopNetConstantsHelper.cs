﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoopNet.Models.Helpers
{
    public static class LoopNetConstantsHelper
    {
        public const string LegacyNftFactoryContract = "0xc852aC7aAe4b0f0a0Deb9e8A391ebA2047d80026";
        public const string CurrentNftFactoryContract = "0x97BE94250AEF1Df307749aFAeD27f9bc8aB911db";
        public const string ExchangeAddress = "0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4";

        public const string ProductionLoopringApiEndpoint = "https://api3.loopring.io";
        public const string GetTickersApiEndpoint = "api/v3/ticker";
        public const string GetMarketsApiEndpoint = "api/v3/exchange/markets";
        public const string GetAccountInformationApiEndpoint = "api/v3/account";
        public const string GetApiKeyApiEndpoint = "api/v3/apiKey";
        public const string GetStoragIdApiEndpoint = "api/v3/storageId";
        public const string GetOffchainFeeApiEndpoint = "api/v3/user/offchainFee";
        public const string GetOffchainFeeNftApiEndpoint = "api/v3/user/nft/offchainFee";
        public const string GetCounterFactualNftTokenAddressApiEndpoint = "api/v3/nft/info/computeTokenAddress";
        public const string GetNftCollectionInfoApiEndpoint = "api/v3/nft/collection";


        public const string PostTokenTransferApiEndpoint = "api/v3/transfer";
        public const string PostNftMintApiEndpoint = "api/v3/nft/mint";
    }
}