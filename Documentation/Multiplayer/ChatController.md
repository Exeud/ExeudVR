# ChatController
[ChatController.cs](../../Assets/ExeudVR/Scripts/Multiplayer/ChatController.cs)

## Description

ChatController manages the sending and receiving of text data over the network, providing a user interface for chat functionality in the ExeudVR system.

## Public Variables, Functions, and Attributes

- `HasFocus` (bool): Indicates whether the chat input field currently has focus.
- `GetFocus()`: Activates the chat input field.
- `LoseFocus()`: Deactivates the chat input field.
- `BroadcastChatMessage()`: Sends the current chat message to all connected users.
- `UpdateChatFeed(string id, AvatarChatData acd)`: Updates the chat display with new messages.
- `UpdateChatMessage(string message)`: Updates the current draft message in the input field.

## Serialized Fields

- `_input` (InputField): Reference to the UI input field for chat messages.
- `_output` (Text): Reference to the UI text component for displaying chat messages.

## Private Variables

- `currentDraft` (string): The current message being composed.
- `messageBuffer` (string): Temporary storage for incoming messages.
- `newChatMessageReady` (bool): Flag indicating a new message is ready to be displayed.

## Events

- `OnBroadcastMessage` (ChatBroadcast): Event triggered when a chat message is broadcasted.

## Methods

- `Update()`: Handles input for sending messages and updates the chat display.

## How it Works

1. The ChatController manages a simple chat system within the ExeudVR environment.

2. Users can focus on the chat input field to type messages. The `HasFocus` property tracks whether the input field is active.

3. When the Enter key is pressed and the input field has focus, the current message is broadcasted using `BroadcastChatMessage()`.

4. Incoming messages are processed by `UpdateChatFeed()`, which adds them to a message buffer.

5. The `Update()` method continuously checks for new messages in the buffer and updates the chat display accordingly.

6. The `UpdateChatMessage()` method handles real-time updates to the current draft message, including a special character (`) for potential command processing.

7. The `OnBroadcastMessage` event allows other components to react to outgoing chat messages, facilitating integration with the network layer.

This controller provides a straightforward way for users to communicate within the virtual environment, integrating with the UI and network systems of ExeudVR.
