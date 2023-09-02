using System;
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

        public const string ProductionLoopringApiEndpoint = "https://api3.loopring.io";
        public const string GetTickersApiEndpoint = "api/v3/ticker";
        public const string GetMarketsApiEndpoint = "api/v3/exchange/markets";
        public const string GetAccountInformationApiEndpoint = "api/v3/account";
        public const string GetApiKeyApiEndpoint = "api/v3/apiKey";
        public const string GetStoragIdApiEndpoint = "api/v3/storageId";
        public const string GetOffchainFeeApiEndpoint = "api/v3/user/offchainFee";
        public const string PostTokenTransferApiEndpoint = "api/v3/transfer";
    }
}
