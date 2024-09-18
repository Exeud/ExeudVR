# RigController
[RigController.cs](../../Assets/ExeudVR/Scripts/Controllers/RigController.cs)

## Description

The RigController acts as an adjustable pivot between the head and hands. It handles body-oriented events and functions, connects the HUD, and manages the placement of the foot plate to keep the user grounded.

## Public Variables, Functions, and Attributes

- None

## Serialized Fields

- `cameraReference` (Transform): Reference to the camera transform.
- `HUDObjectRoot` (GameObject): Root object for the Heads-Up Display (HUD).
- `HUDSnappiness` (float): Controls the responsiveness of the HUD movement. Range: 1f to 10f.
- `footPlate` (GameObject): Reference to the foot plate object.
- `castFootPosition` (bool): Determines whether to use raycasting for foot positioning.
- `footLayerMask` (LayerMask): Layer mask for foot raycasting.

## Private Variables

- `BodyOffset` (Vector3): Offset between the camera and the rig's position.
- `UiOffset` (Vector3): Offset between the HUD and the rig's position.
- `UiStartRot` (Quaternion): Initial rotation of the HUD.
- `footTarget` (Vector3): Target position for the foot plate.
- `xrState` (XRState): Current XR state of the system.

## Methods

- `OnDisable()`: Unsubscribes from the XR state change event.
- `Start()`: Initializes variables and subscribes to the XR state change event.
- `Update()`: Updates the rig's position, rotation, and foot plate placement.
- `FixedUpdate()`: Handles HUD positioning and rotation in non-XR mode.
- `OnXRChange(XRState state)`: Handles changes in XR state.

## How it Works

The RigController is a crucial component in the ExeudVR system, serving as the central point for body orientation and HUD placement:

1. Rig Positioning: It positions itself between the camera (head) and hands, creating a natural body pivot point.

2. HUD Management: In non-XR mode, it dynamically positions and rotates the HUD to follow the user's view, providing a smooth and responsive interface experience.

3. Foot Placement: It manages a foot plate object, which can use raycasting to adapt to terrain, providing a sense of grounding for the user.

4. XR State Adaptation: The controller adapts its behavior based on the current XR state, ensuring appropriate functionality in both XR and non-XR modes.

5. Dynamic Updates: It continuously updates its position and rotation based on the camera's movement, ensuring that the body orientation remains natural and intuitive.

The RigController's adaptive nature allows it to provide a consistent and immersive experience across different XR states and environments, serving as a key component in the ExeudVR interaction model.
