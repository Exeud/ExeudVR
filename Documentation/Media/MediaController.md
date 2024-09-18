# MediaController
[MediaController.cs](../../Assets/ExeudVR/Scripts/Media/MediaController.cs)

## Description

A general purpose media controller, which can be connected to the PressableButtonAction events.

## Public Variables, Functions, and Attributes

- `CurrentTrackInfo` (string): Gets the information of the current track.
- `CurrentTrackNo` (int): Gets the number of the current track.
- `PlayAudio`: Initiates playing of audio tracks.
- `PauseAudio`: Pauses or resumes the currently playing audio.
- `StopAudio`: Stops the currently playing audio.
- `PreviousTrack`: Loads and plays the previous track in the playlist.
- `NextTrack`: Loads and plays the next track in the playlist.
- `UnblockOperation`: Unblocks the track loading operation.
- `LoadAudioTrack` (string): Loads an audio track with the given ID.
- `GetUrlFromWebGL` (string): Receives the URL of an audio file from WebGL.

## External Dependencies

- `GetAudioUrlFromIndexedDB` (string, string): Retrieves the URL of an audio file from IndexedDB.
- `SaveAudioInIndexedDB` (string, string, string): Saves an audio file in IndexedDB.

## Serialized Fields

- `continuousPlay` (bool): Determines if tracks should play continuously.

## Private Variables

- `myAudioSource` (AudioSource): The main AudioSource component for playing tracks.
- `currentTrackList` (string[]): List of current tracks.
- `streamingAssetUrl` (string): URL of the current streaming asset.
- `currentAudioURL` (string): URL of the current audio file.
- `currentAudioId` (string): ID of the current audio file.
- `isLoadingTrack` (bool): Flag indicating if a track is currently being loaded.
- `ReadyToPlay` (bool): Flag indicating if the controller is ready to play.
- `isURLFromWebGLReceived` (bool): Flag indicating if a URL has been received from WebGL.

## Events

- `OnAudioUpdated` (string, int): Event triggered when audio track is updated, passing track info and number.
- `OnTrackChange`: Event triggered when the track changes.

## Methods

- `Start`: Initializes the MediaController.
- `Update`: Handles playing of ready tracks.
- `OnApplicationQuit`: Unloads audio resources on application quit.
- `UpdateTrackText`: Updates the track information.
- `CreateAudioSource`: Creates and configures a new AudioSource component.
- `WaitForTrackEnd`: Coroutine that waits for the current track to end before playing the next.
- `LoadAudioForWebGL`: Coroutine for loading audio in WebGL.
- `Play`: Plays the specified audio file.
- `UnloadAudioResources`: Unloads and destroys current audio resources.
- `LoadClip`: Loads an audio clip from a given URL.
- `LoadAudioFromUri`: Coroutine for loading audio from a URI.
- `LoadAudioInEditor`: Coroutine for loading audio in the Unity editor.

## How it Works

The MediaController manages audio playback in the ExeudVR system, offering a more general-purpose solution compared to the JukeboxController. It interfaces with IndexedDB in WebGL builds for audio file storage and retrieval, and uses Unity's AudioSource for playback.

The controller maintains a list of tracks from a StreamingAsset component and provides functions for playing, pausing, stopping, and navigating through tracks. It supports continuous play, automatically moving to the next track when one finishes.

In WebGL builds, it uses JavaScript interop to handle audio file storage and retrieval from IndexedDB. In the Unity editor, it loads audio directly from file paths. The controller manages the loading and unloading of audio resources and provides events for track changes and updates.

This component is designed to be easily connected to UI elements or other game events, making it versatile for various audio playback scenarios in the ExeudVR environment.
