# EditorSettingsManager
[EditorSettingsManager.cs](../../Assets/ExeudVR/Scripts/Editor/EditorSettingsManager.cs)

## Description

Updates settings from the Setup Window and hard-codes WebXR settings in the Unity Editor.

## Public Variables, Functions, and Attributes

- `TryUpdateWebXRSettings(bool forceUpdate = false)`: Attempts to update WebXR settings, with an option to force the update.

## Serialized Fields

None

## Private Variables

None

## Events

None

## Methods

- `OnDestroy()`: Unsubscribes from the assembly reload event.
- `OnAfterAssemblyReload()`: Calls TryUpdateWebXRSettings after assembly reload.
- `TryUpdateWebXRSettings(bool forceUpdate = false)`: Updates WebXR project settings for compatibility with ExeudVRCameraSet.

## How it Works

EditorSettingsManager is an editor script that manages WebXR settings in the Unity project:

1. Initialization: The script uses the `[InitializeOnLoad]` attribute to ensure it runs when the Unity Editor loads.

2. Assembly Reload Handling: It subscribes to the `AssemblyReloadEvents.afterAssemblyReload` event to update settings after each assembly reload.

3. WebXR Settings Update: The `TryUpdateWebXRSettings` method is the core functionality:
   - It uses `SessionState` to track if settings have been updated, persisting across assembly reloads but clearing when Unity exits.
   - It sets 'Initialize XR on Startup' to true.
   - It configures specific WebXR settings, including reference space types, optional features, and framebuffer settings.

4. Error Handling: The script includes try-catch blocks to handle potential exceptions during the settings update process.

This script ensures that the project's WebXR settings are consistently configured for use with the ExeudVR system, even after code changes or editor restarts. It plays a crucial role in maintaining the correct setup for WebXR development in the ExeudVR project.
