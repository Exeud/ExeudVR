# StreamingAssetHandler
[StreamingAssetHandler.cs](../../Assets/ExeudVR/Scripts/Editor/StreamingAssetHandler.cs)

## Description

Populates streaming asset arrays and serializes paths in the Unity Editor.

## Public Variables, Functions, and Attributes

- `OnInspectorGUI()`: Overrides the default inspector GUI for the StreamingAsset component.

## Serialized Fields

None explicitly declared in this script, but it handles the following serialized properties:
- `extension` (string): File extension for streaming assets.
- `filePaths` (string array): Array of file paths for streaming assets.
- `streamingAssetFolder` (Object): Reference to the streaming asset folder.

## Private Variables

- `kAssetPrefix` (const string): Constant string prefix for asset paths.

## Methods

- `OnEnable()`: Initializes serialized properties.
- `OnInspectorGUI()`: Customizes the inspector GUI for the StreamingAsset component.

## How it Works

StreamingAssetHandler is a custom editor script that enhances the Unity Inspector for StreamingAsset components:

1. Property Initialization: In OnEnable, it initializes serialized properties for the extension, file paths, and streaming asset folder.

2. Custom Inspector: OnInspectorGUI provides a custom interface in the Unity Inspector for the StreamingAsset component.

3. Dynamic Path Population: It automatically populates the filePaths array based on the contents of the specified streaming asset folder and file extension.

4. Path Normalization: The script removes the "Assets/StreamingAssets" prefix from file paths for consistency.

5. Array Management: It dynamically resizes the filePaths array to match the number of files found in the streaming asset folder.

This editor tool streamlines the process of managing streaming assets in the ExeudVR project by automatically detecting and listing relevant files, reducing manual input and potential errors in asset path management.
