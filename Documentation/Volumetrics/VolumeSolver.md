# VolumeSolver
[VolumeSolver.cs](../../Assets/ExeudVR/Scripts/Volumetrics/VolumeSolver.cs)

## Description

Provides volume estimation from mesh data, allowing for accurate calculations of object volumes in the game world.

## Public Variables, Functions, and Attributes

- `VolumeSolver(Vector3 scale)`: Constructor that initializes the VolumeSolver with a given scale.
- `GetMeshVolume(Mesh mesh)`: Calculates and returns the volume of a given mesh.
- `GetMeshVolume(GameObject g)`: Coroutine that calculates and returns the volume of a mesh attached to a GameObject.

## Private Variables

- `ObjScale` (Vector3): Stores the scale of the object being measured.
- `IsBusy` (bool): Indicates whether the solver is currently processing a volume calculation.

## Methods

- `SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)`: Calculates the signed volume of a triangle.
- `GetActiveObject(GameObject currentObject)`: Determines the appropriate GameObject to use for volume calculation.

## How it Works

The VolumeSolver class provides functionality to estimate the volume of 3D objects in the game world. It works by calculating the volume of the mesh associated with a GameObject.

The volume calculation is performed using the signed volumes of triangles method. For each triangle in the mesh, the signed volume is calculated and summed to get the total volume. The absolute value of this sum gives the final volume.

The class handles different scenarios:
1. It can calculate the volume of a given Mesh directly.
2. It can find the appropriate Mesh for a given GameObject (prioritizing the object itself, then its parent, then its children).
3. It accounts for the object's scale in the calculations.
