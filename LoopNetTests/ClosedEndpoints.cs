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
        private string? l1PrivateKey;
        private string? ethAddress;

        [TestInitialize]
        public void TestInitialize()
        {
            string secretsFile = "secrets.json";

            // Assert that the file exists
            Assert.IsTrue(File.Exists(secretsFile), $"Expected secrets file; '{secretsFile}' not found. Please create it in the root of the LoopNetTests project.");

            //Build secrets.json file
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(secretsFile)
                .Build();
            l1PrivateKey = Configuration["Loopring:L1PrivateKey"];
            ethAddress = Configuration["Loopring:Address"];

            // Assert that the keys exist in the configuration
            Assert.IsFalse(string.IsNullOrEmpty(l1PrivateKey), $"The key: 'Loopring:L1PrivateKey' was not found in '{secretsFile}'");
            Assert.IsFalse(string.IsNullOrEmpty(ethAddress), $"The key: 'Loopring:Address' was not found in '{secretsFile}'");
        }

        [TestMethod]
        [Description("Get api key")]
        public async Task GetApiKey()
        {
            var loopringClient = new LoopringClient();
            var accountInformation = await loopringClient.GetAccountInformationAsync(ethAddress!);
            Assert.IsNotNull(accountInformation, "Address not found!");
            if (string.IsNullOrEmpty(accountInformation?.KeySeed))
            {
                accountInformation!.KeySeed = $"Sign this message to access Loopring Exchange: 0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4 with key nonce: {accountInformation.KeyNonce - 1}";
            }
            var messageToSign = accountInformation?.KeySeed;
            int accountId = accountInformation!.AccountId;

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

            Assert.IsNotNull(apiKey?.ApiKey, "Could not get API Key!");
        }
    }
}
