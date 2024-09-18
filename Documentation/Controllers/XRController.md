# XRController
[XRController.cs](../../Assets/ExeudVR/Scripts/Controllers/XRController.cs)

## Description

This script controls the XR controller and handles hand interactions in a virtual reality environment. It manages button events, object grabbing and releasing, and movement control.

## Public Variables, Functions, and Attributes

- `debugHand` (bool): Indicates whether the hand is in debug mode.
- `CharacterRoot` (GameObject): Reference to the root object of the character.
- `BodyController` (BodyController): Reference to the BodyController script component.
- `MaxInteractionDistance` (float): The maximum distance for interactions with objects.
- `GameObjectRenderers` (Renderer[]): Array of Renderers to be toggled based on XR state.
- `hand` (ControllerHand): The controller hand (left or right).
- `PointerLayerMask` (LayerMask): The layer mask for interaction pointers.
- `HandAnchor` (GameObject): Hand anchor used by rigged body model.
- `HandModel` (GameObject): Hand model used by ObjectInterface.
- `IsUsingInterface` (bool): Flag indicating whether the hand is bound to an object.
- `IsControllingObject` (bool): Flag indicating whether the controller is articulating equipment.

## Enumerations

- `ButtonTypes`: Represents the different types of buttons on the controller.
- `AxisTypes`: Represents the different types of axis on the controller (e.g., trigger, grip).
- `Axis2DTypes`: Represents the different types of 2D axes on the controller (e.g., thumbstick, touchpad).

## Events

- `OnControllerActive`: Invoked when the controller is activated or deactivated.
- `OnHandActive`: Invoked when the hand is activated or deactivated.
- `OnHandUpdate`: Invoked when the hand position or rotation is updated.
- `OnObjectTrigger`: Event for object trigger interactions.
- `OnObjectGrip`: Event for object grip interactions.
- `AButtonEvent`: Event for A button press.
- `BButtonEvent`: Event for B button press.
- `OnHandInteraction`: Invoked when hand interaction occurs.

## Methods

- `SetGripPose(string gripPose)`: Sets the grip pose animation trigger.
- `ModifyJoint(int jointIndex, Rigidbody connectedBody = null)`: Modifies a joint between the controller and a connected rigidbody.
- `SetCurrentInterface(bool isControlDelegated, ObjectInterface objInt)`: Sets the current interface and returns the controller hand.
- `PickupFar()`: Picks up a distant object.
- `DropFar()`: Drops the currently held distant object.
- `BeginAttractFar(Rigidbody targetRB)`: Initiates the attraction of a distant object.
- `AttractFar(Wieldable sender)`: Continues the attraction process of a distant object.
- `PickupNear()`: Picks up a nearby object.
- `DropNear()`: Drops the currently held nearby object.

## How it Works

The XRController script manages VR controller interactions in the ExeudVR environment. It handles both hand tracking and controller input, allowing users to interact with objects in the virtual space. The script supports various interaction types, including grabbing, throwing, and using objects at different distances.

Key features include:
1. Dual-mode operation (hand tracking and controller input).
2. Near and far object interaction with physics-based manipulation.
3. Button and axis input handling for VR controllers.
4. Integration with a networked shared asset system for multiplayer interactions.
5. Support for custom grip poses and object-specific interactions.
6. Handling of special interactions like buttons and attractable objects.

The controller continuously updates its state based on input and environmental factors, managing object interactions and movement in the virtual space. It also interfaces with other systems like the BodyController and ObjectInterface to provide a comprehensive VR interaction experience.
