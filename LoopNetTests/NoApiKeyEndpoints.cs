using LoopNet.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoopNetTests
{
    [TestClass]
    public class NoApiKeyEndpoints
    {
        [TestMethod]
        [Description("Get markets test")]
        public async Task GetMarkets()
        {
            var loopringClient = new LoopringClient();
            var markets = await loopringClient.GetMarkets();
            Assert.IsNotNull(markets);
        }

        [TestMethod]
        [Description("Get ticker")]
        public async Task GetTickers()
        {
            var loopringClient = new LoopringClient();
            var tickers = await loopringClient.GetTickers("LRC-ETH");
            Assert.IsNotNull(tickers);
        }
    }
}