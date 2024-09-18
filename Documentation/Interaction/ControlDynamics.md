# ControlDynamics
[ControlDynamics.cs](../../Assets/ExeudVR/Scripts/Interaction/ControlDynamics.cs)

## Description

This component is used to control objects with user interaction. An attached collider, when contacting any of the control event sensors, will emit the relevant control effect. It's useful for user interfaces, switches, or other jointed objects.

## Public Variables, Functions, and Attributes

- `State` (string): Gets the current state of the control.
- `AddControlEvent` (ControlEvent): Adds a new control event to the list.
- `StartInteraction` (GameObject): Starts the interaction with a target object.
- `FinishInteraction`: Finishes the current interaction.
- `ResetPose`: Resets the object to its original pose.

## Serialized Fields

- `resetReference` (Transform): The reference transform for resetting the object's position.
- `stickySensors` (bool): Determines if the sensors should stick to the last contacted object.
- `controlEvents` (List<ControlEvent>): List of control events for this object.

## Private Variables

- `controls` (List<string>): List of control names.
- `contacts` (List<Transform>): List of currently contacting transforms.

## Events

- `onControlAction` (ControlEvent): Event triggered when a control action occurs.

## Methods

- `Awake`: Initializes the control events and sets up the event listener.
- `OnDestroy`: Removes the event listener.
- `InitialiseControlEvents`: Sets up the initial control events and checks their integrity.
- `OnTriggerEnter`: Handles the behavior when a collider enters the trigger zone.
- `OnTriggerExit`: Handles the behavior when a collider exits the trigger zone.
- `StickToTarget`: Makes the object stick to a target transform.
- `ProcessControlEvent`: Processes a control event when triggered.
- `ResetGripOrigin`: Coroutine to reset the object's position and rotation.

## How it Works

The ControlDynamics component manages interactive objects that respond to user input through collider interactions. It maintains a list of ControlEvents, each associating a collider sensor with a specific state and control effect.

When an object with this component interacts with a defined sensor, it triggers the corresponding control event. This can change the object's state and invoke the associated control effect. The component supports "sticky" sensors, where the object can maintain its position relative to the last contacted sensor.

The component also provides methods for starting and finishing interactions, as well as resetting the object's pose. This makes it versatile for creating various interactive elements in a VR environment, such as buttons, switches, or other manipulable objects.

The event system allows for easy extension of functionality, enabling other components to respond to control actions without direct coupling.
