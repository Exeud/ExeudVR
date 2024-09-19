# WalletUI
[WalletUI.cs](../../Assets/ExeudVR/Scripts/WorldComputer/WalletUI.cs)

## Description

Manages coin requests and responses from a single-token EXT canister, handling UI updates and coin spawning.

## Public Variables, Functions, and Attributes

- `BeginTokenRequest()`: Initiates token request process.

## Serialized Fields

- `spawnLocation` (Transform): Location for coin spawning.
- `coinPrefab` (GameObject): Prefab for spawned coins.
- `screenText` (Text): UI text for displaying messages.

## Private Variables

- `sb` (StringBuilder): For building screen messages.
- `isPrinting` (bool): Controls message printing.
- `sbLen` (int): Tracks number of lines in screen message.

## Methods

- `Start()`: Initializes UI components.
- `RequestCoinFromFund(string)`: Requests coin from the fund.
- `onGetCoin(string)`: Processes coin request response.
- `SpawnCoin(string)`: Instantiates coin prefab.
- `UpdateScreenMessage(string, Action<string>, string)`: Updates UI text.
- `RemoveFirstLine()`: Manages text overflow.
- `PrintString(string, Action<string>, string)`: Coroutine for animated text printing.

## How it Works

WalletUI handles the user interface for token requests. It authenticates the user, sends requests to the fund, processes responses, updates the UI with status messages, and spawns coin objects upon successful requests. The class uses ChainAPI for blockchain interactions and includes a test mode for the Unity editor.
