using LoopNet.Services;
using Microsoft.Extensions.Configuration;
using Nethereum.Signer;
using PoseidonSharp;
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

#region Get API Key
var loopringClient = new LoopringClient();

var accountInformation = await loopringClient.GetAccountInformationAsync(ethAddress!);
if (string.IsNullOrEmpty(accountInformation?.KeySeed))
{
    accountInformation!.KeySeed = $"Sign this message to access Loopring Exchange: 0x0BABA1Ad5bE3a5C0a66E7ac838a129Bf948f1eA4 with key nonce: {accountInformation.KeyNonce - 1}";
}
var messageToSign = accountInformation?.KeySeed;
int accountId = accountInformation!.AccountId;

var skipPublicKeyCalculation = false; //set to false to generate the public key details as well, set to true to skip public key generation which makes it run faster

var signer = new EthereumMessageSigner();
var signedMessageECDSA = signer.EncodeUTF8AndSign(messageToSign, new EthECKey(l1PrivateKey));
var l2KeyDetails = LoopringL2KeyGenerator.GenerateL2KeyDetails(signedMessageECDSA, ethAddress, skipPublicKeyCalculation);

//Generating the x-api-sig header details for the get loopring api key endpoint
string apiSignatureBase = "GET&https%3A%2F%2Fapi3.loopring.io%2Fapi%2Fv3%2FapiKey&accountId%3D" + accountId;
BigInteger apiSignatureBaseBigInteger = SHA256Helper.CalculateSHA256HashNumber(apiSignatureBase);
Eddsa eddsa = new Eddsa(apiSignatureBaseBigInteger, l2KeyDetails.secretKey);
var xApiSig = eddsa.Sign();
var apiKey = await loopringClient.GetApiKeyAsync(xApiSig, accountId);
Console.WriteLine($"Your Loopring API Key is: {apiKey?.ApiKey}");
#endregion


var markets = await loopringClient.GetMarketsAsync();
var tickers = await loopringClient.GetTickersAsync("LRC-ETH");

Console.ReadKey();
Console.WriteLine("Press any key to exit!");