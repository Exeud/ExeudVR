# WatchController
[WatchController.cs](../../../Assets/ExeudVR/Scripts/ObjectBehaviour/WatchFunctions/WatchController.cs)

## Description

WatchController manages the functionality of a virtual watch, allowing users to switch between different watch modes or "swatches".

## Public Variables, Functions, and Attributes

- `OnWake()`: Activates the watch and sets it to the current mode.
- `OnSleep()`: Deactivates the watch by setting it to an inactive mode.
- `ChangeMode(float trigVal)`: Cycles to the next available watch mode.

## Serialized Fields

- `modeCanvases` (GameObject[]): Array of canvases representing different watch modes.
- `characterRoot` (GameObject): Reference to the root object of the character wearing the watch.

## Private Variables

- `currentMode` (int): Index of the currently active watch mode.

## Methods

- `SetMode(int newMode)`: Activates the specified watch mode and deactivates others.

## How it Works

The WatchController manages a virtual watch with multiple modes, each represented by a different canvas. When the watch is activated via `OnWake()`, it displays the current mode. The `ChangeMode()` method allows cycling through available modes, incrementing the `currentMode` index and wrapping around when it reaches the end of the available modes.

The `SetMode()` method handles the actual switching between modes, activating the specified canvas and deactivating all others. This creates the effect of changing the watch face or functionality.

This system allows for a versatile and interactive virtual watch interface within the ExeudVR environment, potentially providing various functions or information displays to the user.
