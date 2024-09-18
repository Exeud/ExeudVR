# StreamingAsset
[StreamingAsset.cs](../../Assets/ExeudVR/Scripts/Media/StreamingAsset.cs)

## Description

Identifies the Object as containing media in the `StreamingAssets` folder which is accessed on-demand by the MediaController. This class uses the StreamingAssetHandler in Unity to populate the file paths that will be loaded at runtime.

## Public Variables, Functions, and Attributes

- `currDataIndex` (int): The index of the current data item.
- `BuildDataStore`: Builds and returns an array of file paths for streaming assets.
- `GetCurrentFileUrl`: Returns the filename of the current streaming asset.
- `GetFirstFileUrl`: Returns the URL of the first file in the data store.
- `GetRandomFileUrl`: Returns the URL of a random file from the data store.
- `GetNextFileUrl`: Returns the URL of the next file in the data store.
- `GetPrevFileUrl`: Returns the URL of the previous file in the data store.

## Serialized Fields

- `streamingAssetFolder` (Object): The folder containing streaming assets.
- `extension` (string): The file extension for streaming assets.
- `filePaths` (string[]): Array of file paths for streaming assets.

## Private Variables

- `dataStore` (string[]): Array storing the full paths of streaming assets.

## How it Works

The StreamingAsset component manages access to media files stored in Unity's StreamingAssets folder. It maintains an array of file paths and provides methods to navigate through these files sequentially or randomly. The component initializes by building a data store of full file paths based on the provided file paths and the StreamingAssets path. It offers functionality to get the current, first, next, previous, or a random file URL, making it easy to cycle through or randomly access the available media files. This component is particularly useful for managing collections of audio or video files that need to be accessed at runtime.
