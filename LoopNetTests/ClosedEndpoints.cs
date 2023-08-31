using LoopNet.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoseidonSharp;
using System.Numerics;
using Nethereum.Signer;
using Microsoft.Extensions.Configuration;

namespace LoopNetTests
{
    [TestClass]
    public class ClosedEndpoints
    {
        private IConfiguration? Configuration;

        [TestInitialize]
        public void TestInitialize()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("secrets.json")
                .Build();
        }

        [TestMethod]
        [Description("Get markets test")]
        public async Task GetApiKey()
        {
            var l1PrivateKey = Configuration!["Loopring:L1PrivateKey"];
            var ethAddress = Configuration["Loopring:Address"];
            int accountId = Convert.ToInt32(Configuration["Loopring:AccountId"]);
            var loopringClient = new LoopringClient();
            var accountInformation = await loopringClient.GetAccountInformationAsync(ethAddress);
            var messageToSign = accountInformation?.keySeed;
  
            var skipPublicKeyCalculation = false; //set to false to generate the public key details as well, set to true to skip public key generation which makes it run faster

            var signer = new EthereumMessageSigner();
            var signedMessageECDSA = signer.EncodeUTF8AndSign(messageToSign, new EthECKey(l1PrivateKey));
            var l2KeyDetails = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, ethAddress, skipPublicKeyCalculation);

            //Generating the x-api-sig header details for the get loopring api key endpoint
            string apiSignatureBase = "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" + accountId;
            BigInteger apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
            Eddsa eddsa = new Eddsa(apiSignatureBaseBigInteger, l2KeyDetails.secretKey);
            var xApiSig = eddsa.Sign();
            var apiKey = await loopringClient.GetApiKeyAsync(xApiSig, accountId);

            Assert.IsNotNull(apiKey.apiKey);
        }
    }
}
