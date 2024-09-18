# ObjectInterface
[ObjectInterface.cs](../../Assets/ExeudVR/Scripts/Interaction/ObjectInterface.cs)

## Description

A simplified version of 'Grabbable', for single-handed interaction only. This component allows objects to be manipulated by the user's hands in VR.

## Public Variables, Functions, and Attributes

- `ToggleActivation` (GameObject, bool): Activates or deactivates the object's interaction with a manipulator.
- `SetTrigger` (float): Sets the trigger value for the object when being used or held.
- `SetGrip` (bool): Sets the grip state of the object.

## Serialized Fields

- `controlPoseLeft` (Transform): The pose of the object when held in the left hand.
- `controlPoseRight` (Transform): The pose of the object when held in the right hand.
- `gripPose` (string): The name of the hand pose to use when gripping the object.
- `OnGetFocusEvent` (UnityEvent): Event triggered when the object gains focus.
- `OnLoseFocusEvent` (UnityEvent): Event triggered when the object loses focus.
- `OnGripEvent` (UnityEvent<bool>): Event triggered when the object is gripped or released.
- `OnTriggerEvent` (UnityEvent<float>): Event triggered when the trigger value changes.

## Private Variables

- `previousParent` (Transform): The previous parent of the object before being grabbed.
- `currentManipulator` (GameObject): The current manipulator (hand) interacting with the object.
- `activeHand` (ControllerHand): The currently active hand manipulating the object.
- `IsBeingUsed` (bool): Indicates if the object is currently being used.
- `IsBeingHeld` (bool): Indicates if the object is currently being held.

## Methods

- `OnTriggerEnter`: Handles the object's behavior when a collider enters its trigger zone.
- `OnTriggerExit`: Handles the object's behavior when a collider exits its trigger zone.
- `ReceiveControl`: Sets up the object to be controlled by a manipulator.
- `LoseControl`: Releases the object from the manipulator's control.
- `LerpToControlPose`: Coroutine for smoothly transitioning the object to the control pose.

## How it Works

The ObjectInterface component manages the interaction between VR controllers and manipulable objects in the scene. It handles focus, gripping, and trigger interactions, allowing objects to be picked up, held, and manipulated by the user's virtual hands.

When a controller enters the object's trigger zone, it can be activated for interaction. The object then aligns itself with the appropriate control pose (left or right hand) and can respond to grip and trigger inputs. The component manages the transition of the object between its free state and being held, including adjusting its parent transform and applying the correct hand pose.

The component also provides events that can be used to trigger additional behaviors when the object is focused, gripped, or when the trigger value changes. This allows for flexible and responsive interaction with objects in the VR environment.
