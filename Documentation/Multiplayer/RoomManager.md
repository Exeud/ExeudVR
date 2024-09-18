# RoomManager
[RoomManager.cs](../../Assets/ExeudVR/Scripts/Multiplayer/RoomManager.cs)

## Description

This class manages rooms in a multiplayer space, handling room creation, joining, and updating room information.

## Public Variables, Functions, and Attributes

- `MaxPeers` (int): Gets or sets the maximum number of peers allowed in a room.

## External Dependencies

- `RoomCheck` (sender): Checks for available rooms.
- `OpenRoom` (sender, roomId, roomSize, isPublic): Opens a new room.
- `JoinRoom` (sender, roomId): Joins an existing room.

## Serialized Fields

No explicit serialized fields are present in this class.

## Private Variables

- `rooms` (List<RoomObject>): List of available rooms.
- `fourLW` (TextAsset): Text asset for four-letter words.
- `fiveLW` (TextAsset): Text asset for five-letter words.
- `maxPeers` (int): Maximum number of peers allowed in a room.
- `publicRoom` (bool): Indicates if the room is public.
- `roomString` (string): Current room identifier.
- `joinAnyRoom` (bool): Flag to join any available room.
- `randomSeed` (System.Random): Random number generator for room name creation.

## Events

No explicit events are defined in this class.

## Methods

- `CheckForRooms()`: Initiates a check for available rooms.
- `JoinAnyAvailableRoom()`: Attempts to join any available room.
- `SetRoomSize(int newCapacity)`: Sets the room capacity.
- `SetRoomMode(bool isPublic)`: Sets the room's public/private status.
- `CreateRoom()`: Creates a new room.
- `LeaveRoom()`: Leaves the current room.
- `GetNewRoomName()`: Generates a new room name.
- `OpenOrJoinRoom()`: Opens a new room or joins an existing one.
- `GetAvailableRoom()`: Finds an available room to join.
- `RoomCheckComplete(string message)`: Callback for completed room check.
- `RoomCreated(string roomId)`: Callback for room creation.
- `RoomJoined(string roomId)`: Callback for joining a room.
- `RoomIsFull(string roomId)`: Callback for full room.
- `RoomNotFound(string roomId)`: Callback for non-existent room.
- `RoomFound(string message)`: Callback for found room.

## How it Works

The RoomManager class orchestrates the multiplayer room system:

1. It maintains a list of available rooms and manages room properties like capacity and public/private status.
2. It provides methods to create, join, and leave rooms, as well as to check for available rooms.
3. Room names are generated using a combination of four and five-letter words.
4. It interacts with external systems (likely JavaScript) through DllImport methods for room operations.
5. Callback methods handle various room-related events, updating the local room list and initiating appropriate actions.
6. The class integrates with NetworkIO for network readiness after joining or creating a room.

This system allows for dynamic room management in a multiplayer environment, facilitating user connections and session organization.
