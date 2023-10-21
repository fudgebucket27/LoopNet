using LoopNet.Models.Responses;
using LoopNet.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


#region Setup secrets
string secretsFile = "secrets.json";
if(!File.Exists(secretsFile))
{
    Console.WriteLine($"Expected secrets file; '{secretsFile}' not found. Please create it in the root of the LoopNetConsole project.");
    Console.ReadKey();
    Console.WriteLine("Press any key to exit!");
    return;
}
var Configuration = new ConfigurationBuilder()
        .AddJsonFile(secretsFile)
        .Build();
var l1PrivateKey = Configuration["Loopring:L1PrivateKey"];
var ethAddress = Configuration["Loopring:Address"];
// Assert that the keys exist in the configuration
if(string.IsNullOrEmpty(l1PrivateKey))
{
    Console.WriteLine($"The key: 'Loopring:L1PrivateKey' was not found in '{secretsFile}'. Please create it!");
    Console.ReadKey();
    Console.WriteLine("Press any key to exit!");
    return;
}

if (string.IsNullOrEmpty(ethAddress))
{
    Console.WriteLine($"The key: 'Loopring:Address' was not found in '{secretsFile}'. Please create it!");
    Console.ReadKey();
    Console.WriteLine("Press any key to exit!");
    return;
}
#endregion

var loopNetClient = await LoopNetClient.CreateLoopNetClientAsync(5, l1PrivateKey, ethAddress, true);
//var postMintNftResponse = await loopNetClient.PostNftMintAsync("0x218b4566b14cd8a7f8288601dc285b9e18d1785b", "QmZynjR5a3754EFdtpoZz6rzCvDWtz8q3fJMM9uiKAaLW7", 10, 6, "LRC");
//var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test");


var exchangeTokens = await loopNetClient.GetExchangeTokensAsync();
var minTokenLRC = exchangeTokens.First(x => x.Symbol == "LRC").OrderAmounts.Minimum; //any mount less than minimum can't be traded
var minTokenETH = exchangeTokens.First(x => x.Symbol == "ETH").OrderAmounts.Minimum; //any mount less than minimum can't be traded
Console.WriteLine($"Min LRC: {minTokenLRC}");
Console.WriteLine($"Min ETH: {minTokenETH}");
var orderUserRateAmountResponse = await loopNetClient.GetOrderUserRateAmountAsync("LRC-ETH");
var tradeCostLRC = orderUserRateAmountResponse.Amounts[0].TradeCost;
var tradeCostETH = orderUserRateAmountResponse.Amounts[1].TradeCost;
Console.WriteLine($"Trade cost LRC: {tradeCostLRC}");
Console.WriteLine($"Trade cost ETH: {tradeCostETH}");


//var nftBalanceResponse = await loopNetClient.GetNftWalletBalanceAsync(77900);
//Console.WriteLine($"Nft balance: {nftBalanceResponse!.Count}");
//foreach(var nft in nftBalanceResponse)
//{
//    var nftHoldersResponse = await loopNetClient.GetNftHoldersAsync(nft.Key);
//    Console.WriteLine(JsonConvert.SerializeObject(nftHoldersResponse, Formatting.Indented));
//}

//var postNftTransferResponse = await loopNetClient.PostNftTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "0x10d1635ee4cda45fb7f7ce588765d17e5a1c8e31d8da1dac609849594fad96d0", 1, "LRC", "goerli nft transfer loopnet");
//Console.WriteLine(JsonConvert.SerializeObject(postNftTransferResponse, Formatting.Indented));


//var nftBalanceResponse = await loopNetClient.GetNftTokenIdAsync("0x2a212b36db36d229d3ee5690c7f9fe0099b53d6f05cfb0349060f4c18012a664");
//Console.WriteLine(JsonConvert.SerializeObject(nftBalanceResponse, Formatting.Indented));

//var postMintNftResponse = await loopNetClient.PostNftMintAsync("0x218b4566b14cd8a7f8288601dc285b9e18d1785b", "QmZynjR5a3754EFdtpoZz6rzCvDWtz8q3fJMM9uiKAaLW7", 10, 6, "LRC");
//Console.WriteLine(JsonConvert.SerializeObject(postMintNftResponse, Formatting.Indented));


//var nftCollectionInfo = await loopNetClient.GetNftCollectionInfoAsync("0x0c589fcd20f99a4a1fe031f50079cfc630015184");
//Console.WriteLine(JsonConvert.SerializeObject(nftCollectionInfo, Formatting.Indented));

//var postLegacyNftMintResponse = await loopNetClient.PostLegacyMintNft("QmZynjR5a3754EFdtpoZz6rzCvDWtz8q3fJMM9uiKAaLW7", 10, 6, "LRC");
////Console.WriteLine(JsonConvert.SerializeObject(postLegacyNftMintResponse, Formatting.Indented));

//var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test"); //You probably want to comment this out or change the address to transfer to.....
//Console.WriteLine(JsonConvert.SerializeObject(tokenTransferResponse, Formatting.Indented));

