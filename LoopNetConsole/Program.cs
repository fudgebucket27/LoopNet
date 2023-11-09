using LoopNet.Models.Helpers;
using LoopNet.Models.Requests;
using LoopNet.Models.Responses;
using LoopNet.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Numerics;


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

var loopNetClient = await LoopNetClient.CreateLoopNetClientAsync(1, l1PrivateKey, ethAddress, true);

/// How to mint an Nft Redpacket and send it to a list of targets, 1 nft per packet, 2 packets
//var nftBalance = await loopNetClient.GetNftWalletBalanceAsync();
//var nft = nftBalance!.First().Value.NftData;
//var validSince = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
//var validUntil = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();
//var redpacketNftMintResponse = await loopNetClient.PostNftMintRedPacketAsync(validSince, validUntil, RedPacketType.Common, RedPacketViewType.TARGET, RedPacketAmountType.AVERAGE, nft!, "1", "2", "test", "LRC");
//Console.WriteLine("Nft red packet minted: " + JsonConvert.SerializeObject(redpacketNftMintResponse, Formatting.Indented));
//var addressesToTarget = new List<string> { "0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "0x37ea02537f3a7a7ffc221125245905be3d5423e6" };
//var redpacketNftTargetResponse = await loopNetClient.PostRedPacketTargetAsync(addressesToTarget, redpacketNftMintResponse.Hash, 0);
//Console.WriteLine("Nft red packet sent to targets:" + JsonConvert.SerializeObject(redpacketNftTargetResponse, Formatting.Indented));

/////How to mint a token Redpacket and send it to a list of targets, 1 lrc per packet, 2 packets
var validSince = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
var validUntil = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();
var redpacketMintResponse = await loopNetClient.PostMintRedPacketAsync(validSince, validUntil, RedPacketType.Common, RedPacketViewType.TARGET, RedPacketAmountType.AVERAGE, 1, 1m, 2m, "test token loopnet", "LRC");
Console.WriteLine("Token red packet minted: " + JsonConvert.SerializeObject(redpacketMintResponse, Formatting.Indented));
var addressesToTarget = new List<string> { "0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "0x37ea02537f3a7a7ffc221125245905be3d5423e6" };
var redpacketTargetResponse = await loopNetClient.PostRedPacketTargetAsync(addressesToTarget, redpacketMintResponse.Hash, 0);
Console.WriteLine("Token red packet sent to targets:" + JsonConvert.SerializeObject(redpacketTargetResponse, Formatting.Indented));

////How to do a simple ERC20 trade ETH -> LRC
//var tokens = await loopNetClient.GetExchangeTokensAsync();
//var lrcToken = tokens!.Where(x => x.Symbol == "LRC").First();
//var ethToken = tokens!.Where(x => x.Symbol == "ETH").First();
//var ethSellToken = new Token(tokenId: ethToken.TokenId, amount: 0.03852m, decimals: ethToken.Decimals); //0.038252 ETH
//var lrcBuyToken = new Token(tokenId: lrcToken.TokenId, amount: 368m,  decimals: lrcToken.Decimals); //368 LRC
//var tradeResult = await loopNetClient.PostOrderAsync(
//        sellToken: ethSellToken, //the token to sell
//        buyToken: lrcBuyToken, //the token to buy
//        allOrNone: false, //if partial fills for order are enabled. only false is supported for now by the Loopring API
//        fillAmountBOrS: false, //whether to fill by buy or sell token
//        validUntil: 1800000000, // Unix timestamp for order expiry..
//        maxFeeBips: 63, //maximum order fee
//        clientOrderId: null, //arbitrary client set uniqiue order identifier
//        orderType: OrderType.TAKER_ONLY,
//        tradeChannel: TradeChannel.MIXED
//    );
//Console.WriteLine(JsonConvert.SerializeObject(tradeResult));


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

//var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test");
//Console.WriteLine(JsonConvert.SerializeObject(tokenTransferResponse, Formatting.Indented));

