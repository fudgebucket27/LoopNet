using LoopNet.Services;

var loopringClient = new LoopringClient();
var markets = await loopringClient.GetMarkets();
var tickers = await loopringClient.GetTickers("LRC-ETH");

