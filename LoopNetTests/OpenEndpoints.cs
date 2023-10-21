using LoopNet.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoopNetTests
{
    [TestClass]
    public class OpenEndpoints
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

            _loopNetClient = await LoopNetClient.CreateLoopNetClientAsync(1, _l1PrivateKey, _ethAddress);
        }
        [TestMethod]
        [Description("Get markets")]
        public async Task GetMarkets()
        {
            var markets = await _loopNetClient!.GetMarketsAsync();
            Assert.IsNotNull(markets, "Could not get markets!");
        }

        [TestMethod]
        [Description("Get tickers")]
        public async Task GetTickers()
        {
            var tickers = await _loopNetClient!.GetTickersAsync("LRC-ETH");
            Assert.IsNotNull(tickers, "Could not get tickers!");
        }
    }
}