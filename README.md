# LoopNet
This is the Unofficial Loopring SDK in C# .NET 7

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
