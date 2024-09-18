# MediaPlayerBehaviour
[MediaPlayerBehaviour.cs](../../Assets/ExeudVR/Scripts/Media/MediaPlayerBehaviour.cs)

## Description

A simple script that demonstrates how to connect the DoubleClick function to play and pause either audio or video streams.

## Public Variables, Functions, and Attributes

- `OnDoubleClick`: Toggles audio or video playback when double-clicked.
- `ToggleAudio`: Toggles the playback of an AudioSource component.
- `ToggleVideo`: Toggles the playback of a VideoPlayer component.

## How it Works

The MediaPlayerBehaviour component provides a simple interface for toggling media playback. When the OnDoubleClick method is called, it checks for the presence of either an AudioSource or VideoPlayer component and toggles its playback state accordingly. For audio, it directly plays or pauses the AudioSource. For video, it checks if the VideoPlayer is prepared before toggling between play and pause states. This component can be easily integrated with user interaction systems to provide media control functionality.
