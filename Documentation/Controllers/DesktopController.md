# DesktopController
[DesktopController.cs](../../Assets/ExeudVR/Scripts/Controllers/DesktopController.cs)

## Description

Handles all mouse and keyboard inputs, orients and propels the character in the space. Connects the user to the objects and tools around them and connects to other components.

## Public Variables, Functions, and Attributes

- `CurrentObject` (GameObject): The currently viewed or interacted object.
- `CurrentDistance` (float): The distance to the currently viewed object.
- `CurrentHitPoint` (Vector3): The hit point of the current raycast.
- `IsGameMode` (bool): Indicates if the controller is in game mode.
- `OnNetworkInteraction` (event): Event triggered for network interactions.
- `buttonDown` (bool): Indicates if a button is currently pressed.

## Serialized Fields

- `CharacterVehicle` (GameObject): Character physics container.
- `mouseSensitivity` (float): Mouse sensitivity, range 0.5f to 5.0f.
- `MobileJoystick` (Canvas): Optional joystick for character movement on mobile devices.
- `joystickMultiplier` (float): Joystick sensitivity, when detected.
- `forceJoystickVisible` (bool): Joystick always visible, when detected.
- `pointerLayerMask` (LayerMask): Which layers should the cursor interact with.

## Private Variables

- `_camera` (Camera): The main camera.
- `xrState` (XRState): Current XR state.
- `variableJoystick` (VariableJoystick): Reference to the variable joystick.
- `attachJoint` (FixedJoint): Joint for attaching objects.
- `runFactor` (float): Factor for running speed.
- `jumpCool` (float): Cooldown for jumping.
- `rotationX` and `rotationY` (float): Rotation values for camera movement.
- `currentHeading` (Quaternion): Current camera heading.
- `isMouseDown` and `isDragging` (bool): Mouse state flags.
- `currentSharedAsset` (SharedAsset): Currently interacted shared asset.
- `activeMesh` (GameObject): Currently active mesh for interaction.
- `isNetworkConnected` (bool): Network connection status.
- `currentButton` (PressableButton): Currently pressed button.

## Events

- `OnNetworkInteraction` (CursorInteraction): Triggered when a network interaction occurs.

## Methods

- `Start()`: Initializes the controller.
- `Update()`: Handles per-frame updates.
- `FixedUpdate()`: Handles physics-based updates.
- `OnGUI()`: Handles GUI and input events.
- `MoveBodyWithKeyboard()`: Moves the character based on keyboard input.
- `MoveBodyWithJoystick()`: Moves the character based on joystick input.
- `SetCameraRotation()`: Sets the camera rotation based on input.
- `JumpSwim()`: Handles jumping and swimming actions.
- `PickUpObject(RigidDynamics rd)`: Picks up an object with rigid dynamics.
- `ReleaseObject()`: Releases the currently held object.
- `ScreenRaycast(bool fromTouch = false)`: Performs a raycast from the screen or camera.

## How it Works

The DesktopController manages user input for desktop platforms, handling both mouse/keyboard and optional joystick inputs. It controls character movement, camera rotation, and object interaction. The controller supports different modes (game mode and normal mode) and handles network interactions for multiplayer scenarios. It also manages object picking, throwing, and button interactions, providing a comprehensive control system for desktop VR experiences.
