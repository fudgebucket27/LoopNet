# LoopNet
This is an unofficial Loopring SDK in C# .NET 7

## Important
Use with caution in production. This is being actively developed and every new version will most likely have breaking changes until 1.0.0

## Documentation
You can find the documentation [here](https://fudgey.io/loopnet/index.html).

## Adding to your project
### Installation
You can get LoopNet from nuget. You can run the following command while in Visual Studio to install it into your project

```bash
Install-Package LoopNet
```

## Usage
### Create the client
Create the client as follows. How you retrieve the details for the L1 Private Key and Eth Address is up to you. Be it from a file, environment variable and etc. As long as the final form is a string.

The first parameter is the chain id, it can be either 1 for MAINNET or 5 for TEST.

```csharp
var loopNetClient = await LoopNetClient.CreateLoopNetClientAsync(1, "L1 Private Key", "Eth Address in 0x format");
```
On client creation, it will generate your Loopring Layer 2 Private Key and retrieve your Loopring API Key

### Example usage - Transfer a token
Once the client has been created you can start using the various methods. Here is a token transfer

```csharp
var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test");
```

This will send 0.01 LRC to 0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A with the memo 'LoopNet test', fees will be paid in LRC as well.


### Example usage - Trade a token
Here is a trade of 0.03852 ETH to 368 LRC

```csharp
var tokens = await loopNetClient.GetExchangeTokensAsync();
var lrcToken = tokens!.Where(x => x.Symbol == "LRC").First();
var ethToken = tokens!.Where(x => x.Symbol == "ETH").First();
var ethSellToken = new Token(tokenId: ethToken.TokenId, amount: 0.03852m, decimals: ethToken.Decimals); //0.038252 ETH
var lrcBuyToken = new Token(tokenId: lrcToken.TokenId, amount: 368m, decimals: lrcToken.Decimals); //368 LRC
var tradeResult = await loopNetClient.PostOrderAsync(
        sellToken: ethSellToken, //the token to sell
        buyToken: lrcBuyToken, //the token to buy
        allOrNone: false, //if partial fills for order are enabled. only false is supported for now by the Loopring API
        fillAmountBOrS: false, //whether to fill by buy or sell token
        validUntil: 1800000000, // Unix timestamp for order expiry..
        maxFeeBips: 63, //maximum order fee
        clientOrderId: null, //arbitrary client set uniqiue order identifier
        orderType: OrderType.TAKER_ONLY,
        tradeChannel: TradeChannel.MIXED
    );
```

## Building from source
### Setup
Clone this repo and download Visual Studio 2022 with .NET 7, then open the solution file

### The projects
#### LoopNet
LoopNet is the main class library which contains all of the neccessary methods to interact with the Loopring API.

### LoopNetConsole
LoopNetConsole is a console application that you can use to play around with the methods from LoopNet. It already contains some code so you can see how to use the various methods. See [here](https://github.com/fudgebucket27/LoopNet#running-loopnetconsoleloopnettests) for setup.

### LoopNetTests
LoopNetTests contains all of the tests. See [here](https://github.com/fudgebucket27/LoopNet#running-loopnetconsoleloopnettests) for setup.

## Running LoopNetConsole/LoopNetTests
### Setup secrets.json
You must create a JSON file called 'secrets.json' in the root of the LoopNetTests and the LoopNetConsole projects. It should look like the following:

```json
{
  "Loopring": {
    "L1PrivateKey": "Replace with your L1 Private Key",
    "Address": "Replace with your address in 0x format"
  }
}
```

Do not share your L1 Private Key with anyone!!!

Your address should be in 0x format, for example: 0x36Cd6b3b9329c04df55d55D41C257a5fdD387ACd
