# NodeInputData
[NodeInputData.cs](../../Assets/ExeudVR/Scripts/Multiplayer/NodeInputData.cs)

## Description

This class defines data structures for multiplayer networking, including input data, avatar positions, and audio streaming information.

## Public Variables, Functions, and Attributes

- `NodeInputData` (class): Represents input data from a node.
- `NodeDataFrame` (class): Contains positional and rotational data for avatar components.
- `AvatarEventType` (enum): Defines types of avatar events.
- `ControllerHand` (enum): Specifies which hand is being used.
- `AvatarChatData` (class): Represents chat data for avatars.
- `ConnectionData` (class): Contains connection information.
- `AudioStreamLight` (class): Lightweight audio stream data.
- `AudioStream` (class): Detailed audio stream information.

## Serialized Fields

All classes in this file are marked with the `[Serializable]` attribute. Most properties use `[JsonProperty]` for JSON serialization.

## How it Works

The `NodeInputData` class hierarchy provides a structured way to represent and transmit various types of data in a multiplayer environment:

1. `NodeInputData` represents basic input data from a node, including user ID and latency.
2. `NodeDataFrame` contains detailed positional and rotational data for avatar components (head, hands, pointers).
3. `AvatarEventType` and `ControllerHand` enums provide categorization for events and hand usage.
4. `AvatarChatData` structures chat messages for avatars.
5. `ConnectionData` holds information about user connections.
6. `AudioStream` classes manage audio streaming data.

These classes facilitate the exchange of crucial information in a networked VR environment, enabling synchronized movements, interactions, and communications between users.
