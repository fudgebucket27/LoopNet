using LoopNet.Services;
using Microsoft.Extensions.Configuration;

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

var loopringClient = await LoopringClient.CreateLoopringClientAsync(l1PrivateKey, ethAddress);
var markets = await loopringClient.GetMarketsAsync();
var tickers = await loopringClient.GetTickersAsync("LRC-ETH");

Console.ReadKey();
Console.WriteLine("Press any key to exit!");