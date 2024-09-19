# CursorManager
[CursorManager.cs](../../Assets/ExeudVR/Scripts/Managers/CursorManager.cs)

## Description

Handles cursors in 2D. Changes appearance based on layer. Passes double-clicks to ObjectInterface.

## Public Variables, Functions, and Attributes

- `Instance` (CursorManager): Gets the singleton instance of the CursorManager.
- `isGameMode` (bool): Indicates whether the game is in game mode or not.
- `HandleCursorFocus` (GameObject): Handles the focus of the cursor on objects.
- `DoubleClick`: Triggers a double-click action on the focused object.
- `SetCursorImage`: Sets the cursor image based on the focused object's layer.
- `SetCursorParameters` (XRState): Sets cursor parameters based on the XR state.
- `SetCrosshairVisibility` (bool): Sets the visibility of the crosshair.

## Serialized Fields

- `DebugCursorSelection` (bool): Visualizes pointers in the Editor.
- `cursorForScene` (Texture2D): Default cursor for non-interactive elements.
- `cursorForObjects` (Texture2D): Cursor for things that can be thrown around.
- `cursorForControls` (Texture2D): Cursor for fixed interactables.
- `crosshair` (CentreCrosshair): Reference to the crosshair GameObject.

## Private Variables

- `_instance` (CursorManager): The singleton instance of the CursorManager.
- `hotspot` (Vector2): The hotspot of the cursor.
- `cMode` (CursorMode): The cursor mode.
- `focusedObject` (GameObject): The currently focused object.
- `xrState` (XRState): The current XR state.

## Events

- `OnObjectFocus` (BodyController.CursorFocus): Event triggered when an object gains focus.
- `OnObjectTrigger` (BodyController.ObjectTrigger): Event triggered when an object is triggered.

## Methods

- `Awake`: Sets up the singleton instance and initializes the XR state.
- `Start`: Sets the initial crosshair visibility.
- `Update`: Handles toggling game mode with the 'M' key.
- `OnGUI`: Sets the cursor image if not in editor or debug mode.
- `ToggleGameMode`: Toggles between game mode and normal mode.
- `SetDefaultCursor`: Sets the default cursor and crosshair appearance.
- `SetCursorImageFromLayer`: Sets the cursor image based on the layer of the focused object.

## How it Works

The CursorManager is responsible for managing the cursor's appearance and behavior in the game. It changes the cursor's image based on the layer of the object it's hovering over, providing visual feedback to the user about what they can interact with.

In game mode, it manages a crosshair instead of a cursor, adjusting its appearance based on what the player is looking at. The manager also handles the transition between normal and game modes, which affects cursor visibility and behavior.

The component interfaces with the ObjectInterface system, managing focus on interactive objects and triggering actions like double-clicks. It also provides events that other components can subscribe to for cursor-related actions.

The CursorManager adapts to different XR states, ensuring appropriate cursor behavior across various VR and non-VR scenarios.
