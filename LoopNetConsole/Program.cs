using LoopNet.Services;

var loopringClient = new LoopringClient();
var markets = await loopringClient.GetMarketsAsync();
var tickers = await loopringClient.GetTickersAsync("LRC-ETH");

