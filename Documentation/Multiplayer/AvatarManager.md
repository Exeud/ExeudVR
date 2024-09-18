# AvatarManager
[AvatarManager.cs](../../Assets/ExeudVR/Scripts/Multiplayer/AvatarManager.cs)

## Description

AvatarManager handles the visualization of avatars and processes network events related to avatar presence and movement in the ExeudVR system.

## Public Variables, Functions, and Attributes

- `Instance` (AvatarManager): Singleton instance of AvatarManager.
- `AudioChannelOpen` (bool): Indicates whether the audio channel is open.
- `ProcessAvatarData(NodeInputData nodeFrame)`: Processes incoming avatar data.
- `RemovePlayerAvatar(string userId)`: Removes an avatar from the scene.
- `ResetScene()`: Clears all avatars and resets the manager state.

## Serialized Fields

- `avatarTemplate` (GameObject): Prefab used for instantiating new avatars.

## Private Variables

- `avatarControllers` (Dictionary<string, AvatarController>): Stores active avatar controllers.
- `readyToCreateAvatar` (bool): Flag indicating readiness to create a new avatar.
- `currentDataFrame` (NodeDataFrame): Temporary storage for incoming avatar data.

## Events

- `OnDictionaryChanged` (DictionaryChanged): Event triggered when the number of active avatars changes.

## Methods

- `CreateNewPlayerAvatar(NodeDataFrame nodeFrame)`: Instantiates a new avatar based on received data.

## How it Works

AvatarManager creates and updates avatars based on network data. It instantiates new avatars when users join, updates their positions and states with incoming data frames, and removes avatars when users leave. The manager maintains a dictionary of active avatars and notifies other components when the number of avatars changes. It integrates with the NetworkIO system to process incoming data and the Unity scene to visualize avatars.
