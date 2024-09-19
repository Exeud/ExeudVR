# XRPointer
[XRPointer.cs](../../Assets/ExeudVR/Scripts/Controllers/XRPointer.cs)

## Description

Handles raycast and pointer placement for the parent hand in the ExeudVR system.

## Public Variables, Functions, and Attributes

- `PlacePointer()`: Places the pointer and returns the current object being pointed at.

## Serialized Fields

- `MaxInteractionDistance` (float): Maximum distance for interaction raycast.
- `PointerLayerMask` (LayerMask): Layer mask for pointer interaction.
- `PointerLine` (Transform): Transform of the pointer line visual.

## Private Variables

- `currentObject` (GameObject): The object currently being pointed at.
- `parentHand` (Transform): Reference to the parent hand's transform.
- `HasRingLock` (bool): Experimental feature for wider selection area at a greater distance.

## Methods

- `Start()`: Initializes the parent hand reference.
- `PlacePointer()`: Updates the pointer's position and rotation, returns the current object.
- `CastControllerRay()`: Performs the raycast from the controller and returns the hit point.

## How it Works

The XRPointer script is responsible for handling the pointer functionality in the ExeudVR system:

1. Initialization: In Start, it sets up the reference to the parent hand.

2. Pointer Placement: The PlacePointer method is the main public interface. It updates the pointer's position and rotation based on the raycast result and returns the object being pointed at.

3. Raycasting: The CastControllerRay method performs the actual raycast from the controller. It uses the MaxInteractionDistance and PointerLayerMask to determine valid hits.

4. Object Detection: It keeps track of the current object being pointed at, which can be used by other scripts for interaction purposes.

5. Visual Feedback: The script positions and rotates the PointerLine to provide visual feedback of the pointer's direction.

6. Experimental Features: There's a commented-out section for a 'RingLock' feature, which could potentially provide a wider selection area at greater distances.

This script provides pointing capabilities, essential for object selection and interaction in the VR environment.
