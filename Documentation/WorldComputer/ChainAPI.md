# ChainAPI
[ChainAPI.cs](../../Assets/ExeudVR/Scripts/WorldComputer/ChainAPI.cs)

## Description

Handles callbacks from browser scripting interactions and provides an interface for Internet Computer (IC) operations.

## Public Variables, Functions, and Attributes

- `Instance` (ChainAPI): Singleton instance of ChainAPI.
- `currentProfile` (UserProfile): Reference to the current user profile.
- `HandleCallback` (string): Processes JSON callback data from browser interactions.
- `FinaliseCallback` (int): Removes a callback from the tracking system.
- `ICLogin` (Action<string>): Initiates IC login process.
- `ICLogout` (Action<string>): Ends IC session.
- `RequestToken` (Action<string>): Requests a token from the fund.

## External Dependencies

None.

## Serialized Fields

None.

## Private Variables

- `_instance` (ChainAPI): Private reference for the singleton instance.
- `callbacks` (Dictionary<int, Action<string>>): Tracks callback actions.
- `cbIndex` (int): Index for managing callbacks.

## Events

None.

## Methods

- `Awake`: Initializes the singleton instance.
- `GetCallbackIndex` (Action<string>): Generates and manages callback indices.
- `HandleCallback` (string): Processes JSON callback data.
- `FinaliseCallback` (int): Removes a completed callback.
- `ICLogin` (Action<string>): Initiates IC login.
- `ICLogout` (Action<string>): Ends IC session.
- `RequestToken` (Action<string>): Requests a token.

## How it Works

ChainAPI serves as a central manager for Internet Computer interactions in ExeudVR. It uses a singleton pattern to ensure a single point of access throughout the application. The class manages callbacks using a dictionary and index system, allowing for asynchronous communication with browser scripts.

The API provides methods for IC login, logout, and token requests, which are likely used for user authentication and managing digital assets. These methods utilize the CanisterUtils and TokenUtils classes to interact with the IC infrastructure.

Error handling is implemented in the HandleCallback method, which processes JSON responses from browser interactions. It includes error logging and utilizes the ChainUtils.InterrogateCanisterResponse method for detailed error analysis.

The class also includes a StringExt static class with extension methods for string truncation and "book-ending", which may be used for formatting long strings like blockchain addresses or transaction hashes.
