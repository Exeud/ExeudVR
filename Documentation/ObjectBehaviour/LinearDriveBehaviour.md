# LinearDriveBehaviour
[LinearDriveBehaviour.cs](../../Assets/ExeudVR/Scripts/ObjectBehaviour/LinearDriveBehaviour.cs)

## Description

LinearDriveBehaviour controls the linear movement of a drive arm within specified limits, allowing for positive and negative actuation.

## Public Variables, Functions, and Attributes

- `ActuatePositive()`: Initiates forward movement of the drive arm.
- `ActuateNegative()`: Initiates reverse movement of the drive arm.
- `Halt()`: Stops the movement of the drive arm.

## Serialized Fields

- `DriveArm` (Rigidbody): Reference to the Rigidbody component of the drive arm.
- `MaxActuation` (float): Maximum position limit for the drive arm.
- `MinActuation` (float): Minimum position limit for the drive arm.

## Private Variables

- `forwardDrive` (bool): Flag indicating forward movement.
- `reverseDrive` (bool): Flag indicating reverse movement.

## Methods

- `Update()`: Handles the continuous movement of the drive arm based on actuation flags.

## How it Works

LinearDriveBehaviour controls a drive arm's linear motion using Unity's physics system. It checks movement flags in the `Update` method, moving the arm up or down within specified limits using the Rigidbody's `MovePosition` method. External control is provided through `ActuatePositive()`, `ActuateNegative()`, and `Halt()` methods, which set movement flags. This behavior enables controlled linear motion for interactive elements in the ExeudVR environment.
