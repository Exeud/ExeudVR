# SharedAssetManager
[SharedAssetManager.cs](../../Assets/ExeudVR/Scripts/SharedAssets/SharedAssetManager.cs)

## Description

SharedAssetManager is a singleton component that manages the shared asset register, tracking objects whose movement and state are shared across the p2p network.

## Public Variables, Functions, and Attributes

- `Instance` (SharedAssetManager): Gets the singleton instance of SharedAssetManager.
- `SharedAssetRegister` (Dictionary<string, GameObject>): Gets the dictionary storing shared assets.
- `RetrieveAssetFromRegister(string id)`: Retrieves an asset from the register by its ID.
- `IncludeAssetInRegister(string Id, GameObject asset)`: Adds a new asset to the register.
- `RemoveAssetFromRegister(string Id)`: Removes an asset from the register.
- `UpdateSharedAsset(string Id, GameObject asset)`: Updates an existing asset in the register.

## Private Variables

- `_instance` (SharedAssetManager): Stores the singleton instance.

## Methods

- `Awake()`: Initializes the singleton instance and the SharedAssetRegister.

## How it Works

SharedAssetManager initializes as a singleton, creating a dictionary to store shared assets. It provides methods to add, remove, retrieve, and update assets in the register. The singleton pattern ensures a single point of access for managing shared assets across the ExeudVR network. When adding or updating assets, it uses the provided ID as a key in the dictionary, allowing for efficient retrieval and manipulation of shared objects.
