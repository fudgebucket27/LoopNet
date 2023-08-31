using LoopNet.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LoopNetTests
{
    [TestClass]
    public class OpenEndpoints
    {
        [TestMethod]
        [Description("Get markets")]
        public async Task GetMarkets()
        {
            var loopringClient = new LoopringClient();
            var markets = await loopringClient.GetMarketsAsync();
            Assert.IsNotNull(markets);
        }

        [TestMethod]
        [Description("Get tickers")]
        public async Task GetTickers()
        {
            var loopringClient = new LoopringClient();
            var tickers = await loopringClient.GetTickersAsync("LRC-ETH");
            Assert.IsNotNull(tickers);
        }
    }
}