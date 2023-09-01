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
        private IConfiguration? _Configuration;
        private string? _l1PrivateKey;
        private string? _ethAddress;
        private LoopNetClient? _loopNetClient;
        [TestInitialize]
        public async Task TestInitialize()
        {
            string secretsFile = "secrets.json";

            // Assert that the file exists
            Assert.IsTrue(File.Exists(secretsFile), $"Expected secrets file; '{secretsFile}' not found. Please create it in the root of the LoopNetTests project.");

            //Build secrets.json file
            _Configuration = new ConfigurationBuilder()
                .AddJsonFile(secretsFile)
                .Build();
            _l1PrivateKey = _Configuration["Loopring:L1PrivateKey"];
            _ethAddress = _Configuration["Loopring:Address"];

            // Assert that the keys exist in the configuration
            Assert.IsFalse(string.IsNullOrEmpty(_l1PrivateKey), $"The key: 'Loopring:L1PrivateKey' was not found in '{secretsFile}'");
            Assert.IsFalse(string.IsNullOrEmpty(_ethAddress), $"The key: 'Loopring:Address' was not found in '{secretsFile}'");
            
            _loopNetClient = await LoopNetClient.CreateLoopringClientAsync(_l1PrivateKey, _ethAddress);
        }

        [TestMethod]
        [Description("Get storage id")]
        public async Task GetStorageId()
        {
            var storageId = await _loopNetClient!.GetStorageId(1);
            Assert.IsNotNull(storageId, "Could not get storage id");
        }

        [TestMethod]
        [Description("Get offchain fee")]
        public async Task GetOffchainFee()
        {
            var offchainFee = await _loopNetClient!.GetOffchainFee(15, "LRC", "1");
            Assert.IsNotNull(offchainFee, "Could not get offchain fee");
        }
    }
}
