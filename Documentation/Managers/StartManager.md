# StartManager
[StartManager.cs](../../Assets/ExeudVR/Scripts/Managers/StartManager.cs)

## Description

Links and handles events linked to stepping off the start pad.

## Public Variables, Functions, and Attributes

- `Instance` (StartManager): Gets the singleton instance of the StartManager.

## Serialized Fields

- `OnInitialisation` (UnityEvent): Event triggered when the player steps off the start pad.

## Private Variables

- `_instance` (StartManager): The singleton instance of the StartManager.
- `hasStarted` (bool): Indicates whether the player has stepped off the start pad.

## Events

- `OnInitialised`: Event triggered when the player steps off the start pad.

## Methods

- `Awake`: Sets up the singleton instance.
- `OnTriggerExit`: Detects when the player steps off the start pad and triggers the initialization events.

## How it Works

The StartManager is a singleton class that manages the initialization of the game when the player steps off the start pad. It uses a trigger collider to detect when the player (identified by the presence of a Camera component) exits the start area.

When the player steps off the start pad, the `OnTriggerExit` method is called. If this is the first time the player has stepped off (checked via the `hasStarted` flag), it triggers two events:

1. The `OnInitialised` event, which other scripts can subscribe to for custom initialization logic.
2. The `OnInitialisation` UnityEvent, which can be set up in the Unity Inspector for easy integration with other GameObjects and components.

This setup allows for a centralized way to manage game initialization, ensuring that certain actions or game states are only triggered once the player has actively started the game by moving off the start pad.
