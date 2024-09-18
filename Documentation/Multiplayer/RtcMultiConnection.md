# RtcMultiConnection
[RtcMultiConnection.cs](../../Assets/ExeudVR/Scripts/Multiplayer/RtcMultiConnection.cs)

## Description

This class defines the structure for WebRTC multi-connection functionality, including peer connections, session management, and various configuration options for real-time communication.

## Public Variables, Functions, and Attributes

This file primarily contains serializable classes that represent different aspects of WebRTC connections. The main class is `RtcMultiConnection`, which includes numerous properties for configuring and managing WebRTC connections.

- `RtcMultiConnection` (class): The main class containing all WebRTC connection configuration properties.
- `RoomObject` (class): Represents a room in the WebRTC connection system.

## Serialized Fields

The classes in this file use the `[Serializable]` attribute, which allows them to be serialized for data transfer or storage. Most fields within these classes are decorated with `[JsonProperty]` attributes for JSON serialization.

## Private Variables

This file doesn't contain explicit private variables. All properties are public within their respective classes.

## Events

No explicit events are defined in this file.

## Methods

No methods are defined in this file. The classes primarily consist of properties.

## How it Works

The `RtcMultiConnection` class serves as a comprehensive configuration object for WebRTC connections. It includes properties for session management, peer connections, media constraints, ICE servers, and various other settings related to real-time communication.

The `RoomObject` class represents a room in the WebRTC system, containing information about participants, session details, and room status.

These classes are likely used in conjunction with JavaScript libraries or other C# scripts to facilitate WebRTC connections in a Unity-based multiplayer system. The serializable nature of these classes allows for easy conversion between C# objects and JSON data, which is crucial for network communication and data persistence in WebRTC applications.
