# FollowerBehaviour
[FollowerBehaviour.cs](../../Assets/ExeudVR/Scripts/ObjectBehaviour/FollowerBehaviour.cs)

## Description

FollowerBehaviour manages VR toggle and settings functionality, handling settings animation and visibility.

## Public Variables, Functions, and Attributes

- `settingsOpen` (bool): Indicates whether the settings pane is currently open.
- `ToggleSettings()`: Toggles the visibility and position of the settings pane.
- `ToggleVR()`: Initiates the transition to VR mode.

## Serialized Fields

- `settingsPane` (GameObject): Reference to the settings pane GameObject.

## Private Variables

- `settYPos` (float): Y-position of the settings pane.
- `settYScale` (float): Y-scale of the settings pane.

## Methods

- `MoveAndScalePane(Vector3 targetPos, Vector3 targetScale, float speed)`: Coroutine that animates the movement and scaling of the settings pane.

## How it Works

FollowerBehaviour manages a settings pane in the VR environment. The `ToggleSettings()` method switches the visibility of the settings pane, adjusting its position and scale. When opened, the pane moves to a more visible position (Y: 0.35) and scales up (Y: 0.3). When closed, it moves to a less obtrusive position (Y: 0.15) and scales down (Y: 0.0).

The `MoveAndScalePane()` coroutine smoothly animates these transitions, creating a fluid user experience. It uses Vector3.Lerp and Mathf.SmoothStep for smooth interpolation of position and scale.

The `ToggleVR()` method interfaces with the PlatformManager to initiate the transition to VR mode.

This behaviour allows for an interactive and visually appealing settings interface in the ExeudVR environment, enhancing user experience and providing easy access to VR mode toggling.
