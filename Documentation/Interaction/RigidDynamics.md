# RigidDynamics
[RigidDynamics.cs](../../Assets/ExeudVR/Scripts/Interaction/RigidDynamics.cs)

## Description

This component allows the object to be thrown with a force based on the hand velocity and weight of the object. It automatically updates the object's mass based on density and calculates its volume if the mesh is readable.

## Public Variables, Functions, and Attributes

- `Mass` (float): Gets or sets the mass of the object.
- `Volume` (float): Gets or sets the volume of the object.
- `Throw` (ThrowData): Gets or sets the forces applied to the rigid body during a throw.
- `UsesGravity` (bool): Gets whether the rigid body uses gravity.

## Serialized Fields

- `Density` (float): The density of the object in g/cm3 [0.001 - 10.000]. Assumes isentropy. Water = 1.0, Air = 0.00123, Iron = 7.874.
- `targetMesh` (MeshFilter): Target for volume estimation.

## Private Variables

- `_mass` (float): Stores the mass of the object.
- `_volume` (float): Stores the volume of the object.
- `_usesGravity` (bool): Stores whether the object uses gravity.
- `_throw` (ThrowData): Stores the throw data.
- `myRB` (Rigidbody): Reference to the attached Rigidbody component.

## Methods

- `OnEnable`: Initializes the component and sets up the Rigidbody reference.
- `Start`: Resets the component and initializes mass, volume, and gravity settings.
- `ResetComponent`: Resets the component to its default state.
- `GetVolume`: Calculates and returns the volume of the object.
- `GetMesh`: Retrieves the mesh of the object.
- `GetNewMass`: Calculates and sets the new mass based on density and volume.
- `FixedUpdate`: Updates the object's trajectory and rotation data.
- `GetCurrentVelocity`: Calculates and returns the current velocity of the object.
- `GetNormalisedAngVel`: Calculates and returns the normalized angular velocity.
- `ToAngularVelocity`: Converts a quaternion to angular velocity.
- `GetForcesOnRigidBody`: Calculates and returns the forces applied to the rigid body during a throw.

## How it Works

The RigidDynamics component manages the physical properties and behavior of objects in the ExeudVR system. It automatically calculates the object's mass based on its density and volume, which can be derived from the object's mesh if available. The component tracks the object's movement and rotation, allowing for realistic throwing mechanics.

During each physics update (FixedUpdate), the component calculates the object's current velocity and angular velocity. When thrown, it computes the linear and angular forces applied to the object based on its recent movement history. This allows for realistic throwing behavior that takes into account the object's mass and the user's throwing motion.

The component also manages the object's interaction with gravity and provides methods to reset the object to its default state. By handling these physics calculations, RigidDynamics enables more realistic and interactive object behavior in the virtual environment.
