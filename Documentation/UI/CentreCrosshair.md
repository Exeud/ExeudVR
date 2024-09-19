# CentreCrosshair
[CentreCrosshair.cs](../../Assets/ExeudVR/Scripts/UI/CentreCrosshair.cs)

## Description

Handles the crosshair appearance for 'Game Mode', allowing dynamic customization of the crosshair's properties.

## Public Variables, Functions, and Attributes

- `SetColor(CrosshairColorChannel channel, int value, bool redrawCrosshair)`: Sets the color of a specific channel of the crosshair.
- `SetActive(bool newState)`: Activates or deactivates the crosshair canvas.
- `SetColor(Color color, bool redrawCrosshair)`: Sets the overall color of the crosshair.
- `SetThickness(int newThickness, bool redrawCrosshair)`: Sets the thickness of the crosshair lines.
- `SetSize(int newSize, bool redrawCrosshair)`: Sets the size of the crosshair.
- `SetGap(int newGap, bool redrawCrosshair)`: Sets the gap between the center and the crosshair lines.
- `GetSize()`: Returns the current size of the crosshair.
- `GetThickness()`: Returns the current thickness of the crosshair lines.
- `GetGap()`: Returns the current gap of the crosshair.
- `GetColor()`: Returns the current color of the crosshair.
- `GetCrosshair()`: Returns the current Crosshair object.

## Serialized Fields

- `m_crosshair` (Crosshair): Contains properties that specify how the crosshair looks.
- `m_crosshairImage` (Image): Specifies the image to draw the crosshair to.

## Private Variables

- `canvas` (GameObject): The canvas object containing the crosshair image.

## Events

None

## Methods

- `Awake()`: Initializes the crosshair on awake.
- `InitialiseCrosshairImage()`: Sets up the canvas and image for the crosshair if not provided.
- `GenerateCrosshair()`: Generates the crosshair texture and applies it to the image.
- `DrawCrosshair(Crosshair crosshair = null)`: Draws the crosshair texture based on the current settings.
- `DrawBox(int startX, int startY, int width, int height, Texture2D target, Color color)`: Helper method to draw a box on the texture.

## How it Works

The CentreCrosshair class manages a customizable crosshair for the game's UI. It allows for dynamic adjustment of the crosshair's color, size, thickness, and gap. The crosshair is drawn as a texture on a UI Image component.

On Awake, it initializes the crosshair image if not provided and generates the initial crosshair. The class provides methods to modify various aspects of the crosshair appearance, with options to redraw the crosshair immediately after changes.

The crosshair is drawn as four lines (top, right, bottom, left) on a transparent background. The DrawCrosshair method creates the texture, while DrawBox is used to draw each line of the crosshair.

The class also includes getter methods to retrieve current crosshair properties and a method to toggle the visibility of the entire crosshair canvas.
