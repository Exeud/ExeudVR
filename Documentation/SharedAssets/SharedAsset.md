# SharedAsset
[SharedAsset.cs](../../Assets/ExeudVR/Scripts/SharedAssets/SharedAsset.cs)

## Description

SharedAsset is a component that makes a GameObject indexable in the shared asset register and triggers p2p data transfer during interactions.

## Public Variables, Functions, and Attributes

- `IsBeingHandled` (bool): Indicates whether the asset is currently being handled.
- `Id` (string): Unique identifier for the shared asset.
- `DefaultLocation` (Vector3): The default position of the asset.
- `DefaultRotation` (Quaternion): The default rotation of the asset.
- `DefaultScale` (Vector3): The default scale of the asset.

## Private Variables

- `_manager` (SharedAssetManager): Reference to the SharedAssetManager instance.

## Methods

- `SetTransformDefaults()`: Sets the default transform values for the asset.
- `GetGameObjectPath(GameObject obj)`: Static method to get the full path of a GameObject in the hierarchy.

## How it Works

SharedAsset initializes by setting its transform defaults and registering itself with the SharedAssetManager. It generates a unique ID based on the GameObject's hierarchy path. During runtime, it maintains its handled state and default transform values. When destroyed, it removes itself from the shared asset register. This component enables objects to participate in the ExeudVR's shared asset system, facilitating networked interactions and state synchronization.
