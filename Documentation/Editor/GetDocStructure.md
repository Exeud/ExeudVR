# GetDocStructure
[GetDocStructure.cs](../../Assets/ExeudVR/Scripts/Editor/GetDocStructure.cs)

## Description

Generates a JSON file in the documentation root that contains all scripts in the ExeudVR namespace.

## Public Variables, Functions, and Attributes

- `callbackOrder` (int): Gets the callback order for the build process.
- `GenerateStructureManually()`: Static method to manually generate the documentation structure.

## Serialized Fields

None

## Private Variables

None

## Events

None

## Methods

- `OnPreprocessBuild(BuildReport report)`: Generates the structure before the build process.
- `GenerateStructure()`: Main method to generate the JSON structure of the documentation.
- `BuildJsonStructure(string path, StringBuilder json, int indent)`: Recursively builds the JSON structure for a given path.
- `IsInExeudVRNamespace(string filePath)`: Checks if a file is in the ExeudVR namespace.

## How it Works

GetDocStructure is an editor tool that automatically generates a JSON representation of the ExeudVR codebase structure:

1. Build Integration: Implements IPreprocessBuildWithReport to run before each build.

2. Manual Triggering: Provides a menu item to manually generate the structure.

3. JSON Generation: Recursively traverses the Assets/ExeudVR/Scripts directory, creating a JSON structure that mirrors the folder hierarchy.

4. Namespace Filtering: Only includes files that are within the ExeudVR namespace.

5. Output: Generates a STRUCTURE.json file in the Documentation folder, which can be used by documentation tools or AI assistants to understand the project structure.

This tool plays a crucial role in maintaining up-to-date documentation by automatically mapping the project structure, ensuring that the documentation accurately reflects the current state of the codebase.
