# PressableButton
[PressableButton.cs](../../Assets/ExeudVR/Scripts/Interaction/PressableButton.cs)

## Description

PressableButton implements a button that can trigger events when touched or pressed in VR or desktop environments.

## Public Variables, Functions, and Attributes

- `TouchBegin` (UnityEvent): Event triggered when the button is first touched.
- `TouchEnd` (UnityEvent): Event triggered when touch on the button ends.
- `ButtonPressed` (UnityEvent): Event triggered when the button is fully pressed.
- `ButtonReleased` (UnityEvent): Event triggered when the button is released.

## How it Works

The PressableButton class provides a simple interface for creating interactive buttons in a virtual environment. It uses Unity's event system to allow easy connection of button actions to other parts of the application.

When an object interacts with the button, it can trigger four different events:
1. TouchBegin: When the object first makes contact with the button.
2. TouchEnd: When the object stops touching the button.
3. ButtonPressed: When the button is fully pressed down.
4. ButtonReleased: When the button returns to its unpressed state.

These events can be connected to any compatible Unity actions in the inspector or via script, allowing for flexible and reusable button behaviors throughout the application.
