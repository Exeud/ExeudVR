# BodyController

## Description

The BodyController is the meeting point for all data and where they get packaged and sent across the network.

## Public Variables, Functions, and Attributes

- `CurrentUserId` (string): The current user's ID.
- `CurrentNoPeers` (int): The number of peers connected to the network.
- `avatar` (ExeudVRAvatarController): The avatar controller for this body.
- `CharacterHeight` (float): The height of the character.

## External Dependencies

- `SendData` (string): Sends data across the network.

## Serialized Fields

- `headObject` (GameObject): The game object representing the head.
- `bodyObject` (GameObject): The game object representing the body.
- `leftController` (XRController): The left hand controller.
- `rightController` (XRController): The right hand controller.
- `leftPointer` (Transform): The transform of the left pointer.
- `rightPointer` (Transform): The transform of the right pointer.

## Private Variables

- `IsConnectionReady` (bool): Indicates if the connection is ready.
- `nQ` (Queue<string>): Queue for network messages.
- `nQc` (int): Count of messages in the queue.
- `lastTick` (float): Time of the last network update.
- `frameTick` (float): Current frame time for network updates.
- `notifyingNetwork` (bool): Indicates if the network is being notified.

## Events

- `CursorFocus` (GameObject, bool): Event for cursor focus on objects.
- `ObjectTrigger` (ObjectInterface, float): Event for object trigger interactions.
- `ObjectGrip` (ObjectInterface, bool): Event for object grip interactions.

## Methods

- `OnDisable()`: Called when the object becomes disabled.
- `OnXRChange(XRState)`: Handles the XR state change event.
- `GetBodyReference(string)`: Returns the transform for a specified body part.
- `MapControllerEvents(bool)`: Maps or unmaps controller events based on XR state.
- `MapEvents(bool)`: Maps or unmaps various events.
- `Start()`: Called before the first frame update.
- `Update()`: Called once per frame.
- `HandleObjectGrip(ObjectInterface, bool)`: Handles object grip events.
- `HandleObjectTrigger(ObjectInterface, float)`: Handles object trigger events.
- `HandleObjectFocus(GameObject, bool)`: Handles object focus events.
- `GetHandController(ControllerHand)`: Returns the controller for a specified hand.
- `InitialiseDataChannel(string)`: Initializes the data channel for communication.
- `StartAfterDelay(float)`: Starts communication after a delay.
- `playersChanged(int)`: Handles the event when the number of players changes.
- `SetConnectionReady(bool)`: Sets the connection readiness.
- `PackageEventData(AvatarHandlingData)`: Packages the avatar handling data into an event.
- `EnqueuePacket(string)`: Enqueues a packet for sending.
- `SendDataFrame(AvatarEventType, string)`: Sends a data frame across the network.
- `SendPackets()`: Coroutine for sending packets from the queue.

## How it Works

The BodyController class manages body movements, interactions, and network communication in a virtual reality environment. It uses serialized fields to reference game objects and transforms for body tracking, including the head, body, and hand controllers.

The class handles various types of interactions through event delegates: CursorFocus for object focus, ObjectTrigger for trigger interactions, and ObjectGrip for grip interactions. It maps these events to the appropriate controllers based on the current XR state.

For network communication, the BodyController initializes a data channel and manages connection readiness. It uses a queue system (nQ) to handle network messages, sending them at regular intervals. The SendDataFrame method constructs and enqueues data frames containing the current body state and any interaction events.

The class responds to changes in the XR state, adapting the avatar's arm rig accordingly. It also provides methods to get references to different body parts and controllers, allowing for flexible interaction handling.

Overall, the BodyController serves as a central hub for managing and synchronizing body-related data and interactions in a networked virtual reality environment, bridging the gap between local user actions and their representation across the network.
