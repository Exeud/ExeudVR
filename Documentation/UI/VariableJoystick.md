# VariableJoystick
[VariableJoystick.cs](../../Assets/ExeudVR/Scripts/UI/VariableJoystick.cs)

## Description

A customizable joystick component that extends JoystickBase to provide different joystick behaviors.

## Public Variables, Functions, and Attributes

- `MoveThreshold` (float): Gets or sets the movement threshold for the joystick.
- `UpdateJoystickVisibility` (void): Updates the visibility of the joystick background.
- `OnPointerDown` (override): Handles the pointer down event for the joystick.
- `OnPointerUp` (override): Handles the pointer up event for the joystick.

## Serialized Fields

- `moveThreshold` (float): The threshold for joystick movement.
- `joystickType` (JoystickTypeB): The type of joystick behavior (Fixed, Floating, or Dynamic).

## Private Variables

None explicitly defined in the provided code.

## Events

None explicitly defined in the provided code.

## Methods

- `Start` (protected override void): Initializes the joystick.
- `HandleInput` (protected override void): Processes the joystick input based on its type.

## How it Works

The VariableJoystick class extends JoystickBase to provide different joystick behaviors. It supports three types of joysticks: Fixed, Floating, and Dynamic, defined by the JoystickTypeB enum.

The joystick's visibility can be toggled using the UpdateJoystickVisibility method, which is affected by the XRState of the PlatformManager.

When interacted with, the joystick responds differently based on its type:
- For non-Fixed types, the background position is updated on pointer down.
- For Dynamic type, the background position is adjusted based on the input magnitude and move threshold.

The class overrides OnPointerDown and OnPointerUp to handle joystick activation and deactivation, particularly for non-Fixed joystick types.

The HandleInput method processes the joystick input, adjusting the background position for Dynamic joysticks when the input magnitude exceeds the move threshold.
