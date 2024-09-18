# SharedAssetData
[SharedAssetData.cs](../../Assets/ExeudVR/Scripts/SharedAssets/SharedAssetData.cs)

## Description

SharedAssetData defines data structures for shared assets in the ExeudVR system, including enums and classes for avatar interactions and manipulations.

## Public Variables, Functions, and Attributes

- `AvatarInteractionEventType` (enum): Defines types of avatar interaction events (None, AcquireData, ReleaseData).
- `ManipulationDistance` (enum): Specifies the distance of manipulation (None, Near, Far).
- `AvatarHandlingData` (class): Contains data for avatar hand interactions.
- `AcquireData` (class): Represents data for object acquisition events.
- `ReleaseData` (class): Represents data for object release events.

## How it Works

SharedAssetData provides a set of serializable data structures used for network communication in the ExeudVR system. The `AvatarHandlingData` class encapsulates information about hand interactions, including the target object, interaction type, and distance. `AcquireData` and `ReleaseData` classes store specific information for object pickup and release events, including timestamps, positions, and rotations. These structures enable efficient serialization and deserialization of interaction data for network transmission.
