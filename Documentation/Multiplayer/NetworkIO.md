# NetworkIO
[NetworkIO.cs](../../Assets/ExeudVR/Scripts/Multiplayer/NetworkIO.cs)

## Description

NetworkIO is the main entry point for P2P network communication in the ExeudVR system. It manages WebRTC connections, handles network events, and coordinates avatar data transmission, serving as a bridge between C# and JavaScript functionality.

## Public Variables, Functions, and Attributes

- `Instance` (NetworkIO): Singleton instance of NetworkIO.
- `ConnectionState` (enum): Represents the current state of the network connection (NotStarted, Matchmaking, Connected, Disconnected).
- `OpenJoin()`: Initiates joining a room if the connection is established.
- `CloseRTC()`: Closes the current room and connection.
- `MakeReady()`: Prepares the network for data transmission.

## External Dependencies

- `PrimeConnection` (sender, socketURL, capacity): Initializes the WebRTC connection using WebRTCPlugin.jslib.
- `CeaseConnection()`: Terminates the WebRTC connection.
- `ConfigureAudio()`: Sets up audio for the WebRTC connection.

## Serialized Fields

- `SignalingServerUrl` (string): The URL of the signaling server used for matchmaking.
- `connectionIndicator` (Renderer): A visual indicator of the connection state.
- `roomManager` (RoomManager): Component to handle opening and joining rooms.

## Private Variables

- `myConnection` (RtcMultiConnection): The current WebRTC connection object.
- `connectedUsers` (List<string>): List of connected user IDs.
- `previousOwnIds` (List<string>): List of previously used own IDs.
- `ReadyFlag` (bool): Indicates if the network is ready to receive data.
- `CurrentUserId` (string): The current user's ID.

## Events

- `OnNetworkChanged` (NetworkUserEvent): Triggered when the network state changes.
- `OnConnectionChanged` (ConnectionEvent): Triggered when the connection state changes.
- `OnJoinedRoom` (RoomJoinEvent): Triggered when a room is joined.

## Methods

- `OpenConnection()`: Initiates the WebRTC connection using the signaling server.
- `CloseConnection()`: Terminates the WebRTC connection and cleans up resources.
- `ReceivePoseData(string message)`: Handles incoming pose data from other users and forwards it to AvatarManager.
- `OnConnectedToNetwork(string message)`: Processes new network connections and updates the connected users list.
- `OnDisconnectedFromNetwork(string message)`: Handles network disconnections and removes disconnected avatars.
- `DeleteAvatar(string avatarId)`: Removes an avatar from the scene and updates the AvatarManager.

## How it Works

NetworkIO manages the entire lifecycle of network connections in the ExeudVR system:

1. Connection Initialization:
   - It initializes the WebRTC connection using the signaling server via PrimeConnection.
   - The connection state transitions from NotStarted to Matchmaking.

2. Room Management:
   - It interacts with RoomManager to handle room creation, joining, and leaving.
   - Room events (e.g., RoomCreated, RoomJoined) are processed and trigger appropriate actions.

3. Data Flow:
   - Local user data (avatar positions, rotations) is sent to peers using the WebRTC data channel.
   - Incoming data is received through ReceivePoseData and forwarded to AvatarManager for visualization.

4. Avatar Management:
   - NetworkIO coordinates with AvatarManager to add new avatars when users connect.
   - It removes avatars when users disconnect or leave the room.

5. Audio Handling:
   - ConfigureAudio sets up audio streams for connected users.
   - Audio data is processed and distributed to the corresponding avatars.

6. Connection Lifecycle:
   - The ConnectionState enum tracks the connection status throughout the session.
   - NetworkIO handles disconnections and attempts to reconnect when necessary.

7. Performance Considerations:
   - High-frequency updates (like avatar positions) are optimized for efficient transmission.
   - The system uses a ready flag to ensure data is only processed when the network is prepared.

8. Error Handling and Logging:
   - Connection failures and errors are logged for debugging purposes.
   - The system attempts to gracefully handle and recover from network issues.

This class serves as the central hub for all network-related operations, ensuring smooth communication between users in the virtual environment and coordinating with other key components like RoomManager and AvatarManager.
