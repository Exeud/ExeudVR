# Respawn
[Respawn.cs](../../Assets/ExeudVR/Scripts/Managers/Respawn.cs)

## Description

Replaces objects when they fall out of the scene and hit the 'Planes of Destruction'.

## Public Variables, Functions, and Attributes

None specified in the provided code.

## Serialized Fields

- `DefaultRespawnPose` (Transform): The default position and rotation for respawning objects.
- `characterRoot` (GameObject): The root object of the character.

## Private Variables

- `charStartPos` (Vector3): The starting position of the character.

## Methods

- `Awake`: Initializes the character's starting position.
- `OnTriggerEnter`: Detects when an object enters the respawn trigger area.
- `ManageRespawn`: Determines how to handle the respawn based on the object type.
- `ReplaceObject`: Handles the respawning of general objects.
- `ReplaceCharacter`: Handles the respawning of the character.

## How it Works

The Respawn component is responsible for managing the respawn behavior of objects and the character in the ExeudVR environment. It uses a trigger collider to detect when objects fall out of the scene.

When an object enters the trigger area:
1. The `OnTriggerEnter` method is called, which then invokes `ManageRespawn`.
2. `ManageRespawn` determines if the object is part of the character's body or a general object and calls the appropriate respawn method.

For general objects:
- The `ReplaceObject` method is used.
- It checks if the object has a SharedAsset or RigidDynamics component to determine its default position.
- If not, it uses the `DefaultRespawnPose`.
- The object's velocity is reset to prevent continuous falling.

For the character:
- The `ReplaceCharacter` method is used.
- It resets the character's position to its starting position and zeroes out its velocity.

This system ensures that objects and the character don't fall indefinitely out of the playable area, maintaining a consistent and playable environment.
