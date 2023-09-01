# LoopNet
This is an unofficial Loopring SDK in C# .NET 7

## Adding to your project
### Installation
You can get LoopNet from nuget. You can run the following command while in Visual Studio to install it into your project

```bash
Install-Package LoopNet
```

## Usage
### Create the client
Create the client as follows. How you retrieve the details for the L1 Private Key and Eth Address is up to you. Be it from a file, environment variable and etc. As long as the final form is a string.

```csharp
var loopNetClient = await LoopNetClient.CreateLoopNetClientAsync("L1 Private Key", "Eth Address in 0x format");
```
On client creation, it will generate your Loopring Layer 2 Private Key and retrieve your Loopring API Key

### Example usage
Once the client has been created you can start using the various methods. Here is a token transfer

```csharp
var tokenTransferResponse = await loopNetClient.PostTokenTransferAsync("0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A", "LRC", 0.01m, "LRC", "LoopNet test");
```

This will send 0.01 LRC to 0x991B6fE54d46e5e0CEEd38911cD4a8694bed386A with the memo 'LoopNet test', fees will be paid in LRC as well.

## Building from source
### Setup
Download Visual Studio 2022 with .NET 7 and open the solution file

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
