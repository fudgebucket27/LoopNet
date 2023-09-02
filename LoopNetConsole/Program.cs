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

var loopNetClient = await LoopNetClient.CreateLoopNetClientAsync(l1PrivateKey, ethAddress, true);
var postMintNftResponse = await loopNetClient.PostNftMintAsync("0xcef986415766cf34a1d78e2fd8a5bab5f6d02935", "QmYk96iRW6v2p664VqFngzuWD6HDtsr28k9kng238k54cB", 10, 6, "LRC");
Console.WriteLine(JsonConvert.SerializeObject(postMintNftResponse, Formatting.Indented));


//var nftCollectionInfo = await loopNetClient.GetNftCollectionInfoAsync("0x0c589fcd20f99a4a1fe031f50079cfc630015184");
//Console.WriteLine(JsonConvert.SerializeObject(nftCollectionInfo, Formatting.Indented));

//var postLegacyNftMintResponse = await loopNetClient.PostLegacyMintNft("QmYk96iRW6v2p664VqFngzuWD6HDtsr28k9kng238k54cB", 10, 6, "LRC");
//Console.WriteLine(JsonConvert.SerializeObject(postLegacyNftMintResponse, Formatting.Indented));

//var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test"); //You probably want to comment this out or change the address to transfer to.....
//Console.WriteLine(JsonConvert.SerializeObject(tokenTransferResponse, Formatting.Indented));

