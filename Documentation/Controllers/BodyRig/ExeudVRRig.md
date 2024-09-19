# ExeudVRRig
[ExeudVRRig.cs](../../../Assets/ExeudVR/Scripts/Controllers/BodyRig/ExeudVRRig.cs)

## Description

Maps the body rig onto the ExeudVRCameraSet, using the BodyController as information conduit.

## Public Variables, Functions, and Attributes

- `Body` (VRMap): VR mapping for the body.
- `Head` (VRMap): VR mapping for the head.
- `LeftHand` (VRMap): VR mapping for the left hand.
- `RightHand` (VRMap): VR mapping for the right hand.

## Serialized Fields

- `bodyController` (BodyController): Connection to ExeudVR (Location: ExeudVRCameraSet ➥ CharacterRoot ➥ Body).
- `yFac` (float): Y-factor for adjustments (purpose not specified in the code).

## Private Variables

- `headRef` (Transform): Reference to the head transform.
- `bodyRef` (Transform): Reference to the body transform.
- `leftHandRef` (Transform): Reference to the left hand transform.
- `rightHandRef` (Transform): Reference to the right hand transform.

## Classes

### VRMap

A serializable class that handles mapping between VR targets and rig targets.

#### Properties
- `rigTarget` (Transform): The target transform on the rig.
- `trackingPositionOffset` (Vector3): Position offset for tracking.
- `trackingRotationOffset` (Vector3): Rotation offset for tracking.

#### Methods
- `Map(Transform vrTarget)`: Maps the VR target to the rig target, applying offsets.

## Methods

- `Start()`: Initializes references and sets up the avatar controller.
- `Update()`: Updates the mapping for all body parts (head, body, hands) every frame.

## How it Works

The ExeudVRRig script is responsible for mapping the VR input to the avatar's rig:

1. Initialization: In Start, it sets up references to the various body parts using the BodyController.

2. Continuous Mapping: In Update, it continuously maps the VR input (from headRef, bodyRef, and hand references) to the corresponding parts of the avatar rig.

3. Flexible Mapping: The VRMap class allows for flexible mapping with position and rotation offsets, enabling fine-tuning of the avatar's movements.

4. Integration with BodyController: It uses the BodyController to get references to the VR input transforms, ensuring proper coordination with the rest of the ExeudVR system.

This script translates the user's VR movements to the in-game avatar, providing a seamless connection between the player's actions and the virtual representation. 

When adding this to the scene, either as a script or part of a prefab, make sure you connect the `BodyController` component or there will be an error when starting the scene.