# AvatarHand
[AvatarHand.cs](../../Assets/ExeudVR/Scripts/Multiplayer/AvatarHand.cs)

## Description

AvatarHand handles interaction events for the avatar's virtual hand, managing object pickup and release for both near and far interactions.

## Public Variables, Functions, and Attributes

- `ReceiveInstruction(AvatarHandlingData instruction)`: Processes incoming interaction instructions for the hand.

## Private Variables

- `attachJoint` (FixedJoint[]): Array of fixed joints for attaching objects.
- `currentNearRigidBody` (Rigidbody): Currently held near object's rigidbody.
- `currentFarRigidBody` (Rigidbody): Currently held far object's rigidbody.
- `prevLayer` (string): Previous layer of the held object.

## Methods

- `PickupNear(GameObject target, AcquireData acquisition)`: Handles near object pickup.
- `DropNear(GameObject target, ReleaseData release)`: Handles near object release.
- `PickupFar(GameObject target, AcquireData acquisition)`: Handles far object pickup.
- `DropFar(GameObject target, ReleaseData release)`: Handles far object release.
- `SetLayerRecursively(GameObject obj, int newLayer)`: Recursively sets the layer of an object and its children.

## How it Works

AvatarHand manages the interactions between the avatar's hand and objects in the virtual environment. It processes incoming instructions to perform pickup and release actions for both near and far interactions. 

For near interactions, the hand uses a FixedJoint to attach objects, simulating a firm grip. It temporarily changes the object's layer to "Tools" during interaction and restores it upon release. The hand applies forces to the object upon release, simulating throwing or dropping.

Far interactions use a separate joint, allowing for interactions at a distance. These interactions follow a similar pattern to near interactions but without layer changes.

The class integrates with the SharedAssetManager to identify and manipulate shared objects in the multiplayer environment. This ensures that object interactions are synchronized across the network for all users.
