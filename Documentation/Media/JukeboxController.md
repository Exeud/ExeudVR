# JukeboxController
[JukeboxController.cs](../../Assets/ExeudVR/Scripts/Media/JukeboxController.cs)

## Description

Connects to IndexedDB to grab audio from the StreamingAsset component. Links to a Text component to show track info.

## Public Variables, Functions, and Attributes

- `PlayAudio`: Initiates playing of audio tracks.
- `PauseAudio`: Pauses or resumes the currently playing audio.
- `StopAudio`: Stops the currently playing audio.
- `PreviousTrack`: Loads and plays the previous track in the playlist.
- `NextTrack`: Loads and plays the next track in the playlist.
- `LoadAudioTrack` (string): Loads an audio track with the given ID.
- `GetUrlFromWebGL` (string): Receives the URL of an audio file from WebGL.

## External Dependencies

- `GetAudioUrlFromIndexedDB` (string, string): Retrieves the URL of an audio file from IndexedDB.
- `SaveAudioInIndexedDB` (string, string, string): Saves an audio file in IndexedDB.

## Serialized Fields

- `TrackDisplayText` (Text): UI Text component for displaying track information.
- `TrackDisplayNo` (Text): UI Text component for displaying track number.
- `MachineStartSound` (AudioClip): Sound played when the jukebox starts.
- `MachineChangeRecord` (AudioClip): Sound played when changing tracks.

## Private Variables

- `myAudioSource` (AudioSource): The main AudioSource component for playing tracks.
- `machineSounds` (AudioSource): AudioSource for playing jukebox machine sounds.
- `currentTrackList` (string[]): List of current tracks.
- `streamingAssetUrl` (string): URL of the current streaming asset.
- `currentAudioURL` (string): URL of the current audio file.
- `currentAudioId` (string): ID of the current audio file.
- `isLoadingTrack` (bool): Flag indicating if a track is currently being loaded.
- `ReadyToPlay` (bool): Flag indicating if the jukebox is ready to play.
- `isURLFromWebGLReceived` (bool): Flag indicating if a URL has been received from WebGL.

## Methods

- `Start`: Initializes the JukeboxController.
- `Update`: Handles playing of ready tracks.
- `OnApplicationQuit`: Unloads audio resources on application quit.
- `UpdateTrackText`: Updates the UI text with current track information.
- `CreateAudioSource`: Creates and configures a new AudioSource component.
- `LoadAudioForWebGL`: Coroutine for loading audio in WebGL.
- `Play`: Plays the specified audio file.
- `UnloadAudioResources`: Unloads and destroys current audio resources.
- `LoadClip`: Loads an audio clip from a given URL.
- `LoadAudioFromUri`: Coroutine for loading audio from a URI.
- `LoadAudioInEditor`: Coroutine for loading audio in the Unity editor.

## How it Works

The JukeboxController manages the playback of audio tracks in the ExeudVR system. It interfaces with IndexedDB in WebGL builds to store and retrieve audio files, and uses Unity's AudioSource for playback. The controller maintains a list of tracks from a StreamingAsset component and provides functions for playing, pausing, stopping, and navigating through tracks.

In WebGL builds, it uses JavaScript interop to handle audio file storage and retrieval from IndexedDB. In the Unity editor, it loads audio directly from file paths. The controller manages the loading and unloading of audio resources, updates UI elements with track information, and handles the transition between tracks.

The component also includes sound effects for starting the jukebox and changing records, enhancing the user experience. It provides a comprehensive solution for managing and playing audio in both WebGL and Unity editor environments.
