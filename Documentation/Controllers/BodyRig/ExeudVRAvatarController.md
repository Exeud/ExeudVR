# ExeudVRAvatarController
[ExeudVRAvatarController.cs](../../../Assets/ExeudVR/Scripts/Controllers/BodyRig/ExeudVRAvatarController.cs)

## Description

Adjusts hand IK and visibility. Also tracks whether or not the rig is moving and sets animation variables accordingly.

## Public Variables, Functions, and Attributes

- `speedThreshold` (float): The threshold for determining if the avatar is moving.

## Serialized Fields

- `ArmRig` (Rig): Reference to the arm rig for IK control.
- `rightWrist` (Transform): Reference to the right wrist transform.
- `leftWrist` (Transform): Reference to the left wrist transform.

## Private Variables

- `bodyAnimator` (Animator): Reference to the body's animator component.
- `headsetSpeed` (Vector3): Current speed of the headset.
- `headsetLocalSpeed` (Vector3): Local speed of the headset relative to the avatar.
- `prevPos` (Vector3): Previous position of the avatar for speed calculation.

## Methods

- `Start()`: Initializes components and sets up the initial state.
- `PrepareArmRig()`: Prepares the arm rig for IK control, focusing on the "Legs" animation layer.
- `RelaxArmRig()`: Relaxes the arm rig, focusing on the "WholeBody" animation layer.
- `FixedUpdate()`: Updates movement parameters and sets animator variables based on current motion.

## How it Works

The ExeudVRAvatarController manages the avatar's arm IK and movement animations:

1. Arm IK Control: It can switch between IK-controlled arms (PrepareArmRig) and relaxed arms (RelaxArmRig), allowing for dynamic adjustment of the avatar's arm positions.

2. Movement Detection: In FixedUpdate, it calculates the avatar's speed and direction based on position changes.

3. Animation Control: It sets animator parameters based on the calculated movement, including:
   - "isMoving" boolean
   - "directionX" and "directionY" for movement direction
   - "animation_speed" for adjusting animation playback speed

4. Adaptive Animation: The controller ensures that animations adapt to the user's movement, providing a more immersive experience.

5. Performance Optimization: By using FixedUpdate for calculations, it maintains consistent performance regardless of frame rate.

This controller plays a crucial role in making the avatar's movements and animations responsive to the user's actions in the VR environment, enhancing the overall immersion and visual feedback of the ExeudVR system.
