using LoopNet.Models.Helpers;
using LoopNet.Models.Requests;
using LoopNet.Models.Responses;
using LoopNet.Services;
using Microsoft.Extensions.Configuration;
using Nethereum.Util;
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

#region Test
//try
//{
//    var testNftTransferResponse = await loopNetClient.PostNftTransferAsync("0xa2e12f83fca8484c79881f3bc43f7e8d3d08db39", "0x2d6d1c13f9a0e62aa7e1a6e476df7962cc18e48aa3f041f7e3a8a5ae81164f21", 1, "LRC", "GG LSW");
//    Console.WriteLine($"Transfer successful!");
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}
//Console.WriteLine("Test end");
//Console.ReadKey();
//System.Environment.Exit(0);
#endregion

var nftTransferFees = await loopNetClient.GetOffchainFeeNftTransferAsync(19, "0");
var nftTransferFeeLRC = UnitConversion.Convert.FromWei(BigInteger.Parse(nftTransferFees.Fees.Where(x => x.Token == "LRC").First().Fee), 18);
Console.WriteLine($"Current NFT transfer fee: {nftTransferFeeLRC} LRC");
Console.WriteLine("Gathering NFT wallet balance...");
var nfts = await loopNetClient.GetNftWalletBalanceAsync();
var totalNftTransferCost = nfts.Count * nftTransferFeeLRC;
Console.WriteLine($"You have {nfts.Count} NFTS in your wallet");
Console.WriteLine($"Total NFT transfer cost: {totalNftTransferCost} LRC");


string userInput;
string recipientAddress;

do
{
    Console.WriteLine("Enter the Ethereum address to send NFTs to:");
    recipientAddress = Console.ReadLine()?.Trim();

    if (string.IsNullOrEmpty(recipientAddress) || !AddressUtil.Current.IsValidEthereumAddressHexFormat(recipientAddress))
    {
        Console.WriteLine("Invalid Ethereum address. Please try again.");
    }
    else
    {
        Console.WriteLine($"Recipient address: {recipientAddress}");
        break;
    }
} while (true);

do
{
    Console.WriteLine("Transfer all NFTs to the specified wallet? (Y/N)");
    userInput = Console.ReadLine()?.Trim().ToUpper();

    if (userInput == "Y")
    {
        foreach (var nft in nfts)
        {

            string nftName = string.IsNullOrWhiteSpace(nft.Value.Metadata.Base.Name) ? "Name not available" : nft.Value.Metadata.Base.Name;
            Console.WriteLine($"Sending {nftName}");
            try
            {
                var nftTransferResponse = await loopNetClient.PostNftTransferAsync(recipientAddress, nft.Value.NftData, Int32.Parse(nft.Value.Total), "LRC", "GG LSW");
                Console.WriteLine($"Transfer successful!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        break;
    }
    else if (userInput == "N")
    {
        Console.WriteLine("Transfer cancelled.");
        break;
    }
    else
    {
        Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
    }
} while (true);

Console.WriteLine("DONE. Any key to exit!");
Console.ReadKey();
