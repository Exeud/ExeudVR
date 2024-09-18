# AvatarController
[AvatarController.cs](../../Assets/ExeudVR/Scripts/Multiplayer/AvatarController.cs)

## Description

AvatarController acts as the functional center and data hub for each avatar, routing messages to and from the hands, managing audio events, and handling interactions.

## Public Variables, Functions, and Attributes

- `DefaultColour` (Color): Gets the default color of the avatar.
- `Initialise()`: Sets up the avatar's initial state.
- `EndSession()`: Cleans up resources when the avatar session ends.
- `OpenAudioChannel(string userId)`: Starts the audio stream for the avatar.
- `CloseAudioChannel(string userId)`: Stops the audio stream for the avatar.
- `AddAudioStream(string message)`: Processes incoming audio stream data.
- `UpdateAvatar(long latency, NodeDataFrame ndf)`: Updates the avatar's position and state based on network data.
- `ConnectAudioSource(string userid, string audioUrl)`: Connects an audio source to the avatar.
- `SetDefaultColor(Color col)`: Sets the avatar's default color.

## External Dependencies

- `StartAudioStream` (userId): JavaScript function to start an audio stream.
- `StopAudioStream` (userId): JavaScript function to stop an audio stream.

## Serialized Fields

- `Renderers` (List<Renderer>): List of renderers for the avatar's visual components.
- `head` (GameObject): Reference to the avatar's head object.
- `body` (GameObject): Reference to the avatar's body object.
- `leftHand` (AvatarHand): Reference to the left hand controller.
- `rightHand` (AvatarHand): Reference to the right hand controller.
- `leftPointer` (GameObject): Reference to the left hand pointer.
- `rightPointer` (GameObject): Reference to the right hand pointer.
- `latencyText` (TextMesh): Text display for network latency.
- `connectionIndicator` (Renderer): Visual indicator for connection status.

## Private Variables

- `Voice` (AudioSource): Audio source for the avatar's voice.
- `fixedJoint` (FixedJoint): Joint for attaching objects to the avatar.
- `currentRigidbody` (Rigidbody): Currently held object's rigidbody.
- `currentAudioURL` (string): URL of the current audio stream.
- `currentAudioId` (string): ID of the current audio stream.
- `ReadyToPlay` (bool): Flag indicating if audio is ready to play.
- `avatarLerpTime` (float): Time for interpolating avatar movements.

## Methods

- `PickUpObject(GameObject target, AcquireData acquisition)`: Handles object pickup interactions.
- `DropObject(GameObject target, ReleaseData release)`: Handles object release interactions.
- `Play(string fileName, string trackName)`: Plays an audio file.
- `CreateAudioSource(AudioClip clip, string trackId)`: Creates a new audio source for the avatar.
- `UnloadAudioResources()`: Cleans up audio resources.
- `LoadClip(string url, System.Action<AudioClip> onLoadingCompleted)`: Loads an audio clip from a URL.
- `LerpToDataFrame(NodeDataFrame dataFrame, float duration)`: Smoothly interpolates avatar position and rotation.

## How it Works

AvatarController manages the behavior and state of individual avatars in the ExeudVR system. It handles avatar movements by interpolating between received network data frames, ensuring smooth visual representation. The controller processes interaction events, managing object pickups and releases through a fixed joint system. It also handles audio streaming, dynamically loading and playing voice data for each avatar. The controller updates visual elements like color and connection indicators, and manages hand positions and pointers. By centralizing these functions, AvatarController enables cohesive and responsive avatar behavior in the multiplayer environment.