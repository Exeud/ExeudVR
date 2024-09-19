# IIAuth
[IIAuth.cs](../../Assets/ExeudVR/Scripts/WorldComputer/IIAuth.cs)

## Description

Handles Internet Identity authentication for ExeudVR.

## Public Variables, Functions, and Attributes

- `iiUserProfile` (UserProfile): Current user profile.
- `BeginAuth()`: Initiates authentication.
- `EndAuth()`: Ends authentication session.
- `OnGetFocus()`: Handles focus gain.
- `OnLoseFocus()`: Handles focus loss.

## Serialized Fields

- `connectionResult` (UnityEngine.UI.Text): Displays connection status.
- `promptImage` (UnityEngine.UI.Image): Authentication prompt image.

## Private Variables

- `screenUpdateReady` (bool): Flag for screen updates.
- `connectionResultString` (string): Connection status text.
- `authTick` (float): Authentication timing.
- `isIIConnected` (bool): Connection status.

## Methods

- `Start()`: Initializes authTick.
- `Update()`: Updates UI.
- `GetUserProfile()`: Returns current UserProfile.
- `onAuth(string)`: Processes authentication response.
- `OnConfirmExit(string)`: Handles logout confirmation.
- `ProcessAuthResponse(string)`: Parses authentication response.
- `OnDisconnect()`: Sets connection status to false.

## How it Works

IIAuth manages Internet Identity authentication, updating UI elements, processing responses, and maintaining user profile data. It interfaces with ChainAPI for login/logout operations and uses JSON for data parsing.
