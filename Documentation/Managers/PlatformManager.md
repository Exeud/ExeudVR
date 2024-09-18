# PlatformManager
[PlatformManager.cs](../../Assets/ExeudVR/Scripts/Managers/PlatformManager.cs)

## Description

This class looks after mode detection and switching, interfacing with the WebXRManager.

## Public Variables, Functions, and Attributes

- `Instance` (PlatformManager): Gets the singleton instance of the PlatformManager.
- `IsMobile` (bool): Gets or sets whether the platform is mobile.
- `IsVRSupported` (bool): Gets whether VR is supported on the current platform.
- `XrState` (XRState): Gets the current XR state.
- `StartVR`: Initiates VR mode.
- `FormFactorResult` (string): Processes the form factor result from JavaScript.

## External Dependencies

- `DetectFormFactor` (string): JavaScript function to detect the form factor of the device.

## Serialized Fields

None specified in the provided code.

## Private Variables

- `_instance` (PlatformManager): The singleton instance of the PlatformManager.
- `discoveredVR` (bool): Indicates if VR has been discovered.
- `isMobile` (bool): Stores whether the platform is mobile.
- `formFactor` (string): Stores the detected form factor of the device.

## Events

- `OnStateChange` (XRState): Event triggered when the XR state changes.

## Methods

- `Awake`: Sets up the singleton instance.
- `Start`: Checks for VR support on WebGL platform.
- `Update`: Handles VR toggle if VR is discovered.
- `OnEnable`: Subscribes to WebXRManager events.
- `OnDisable`: Unsubscribes from WebXRManager events.
- `CheckCapabilties` (WebXRDisplayCapabilities): Updates VR support status based on WebXR capabilities.
- `OnXRChange` (WebXRState, int, Rect, Rect): Handles XR state changes.

## How it Works

The PlatformManager is for managing different platform states and VR capabilities in the ExeudVR system. It interfaces with the WebXRManager to handle VR mode switching and capability detection.

On startup, it checks for VR support on WebGL platforms. It continuously monitors for VR discovery and can toggle VR mode when necessary. The manager also handles platform-specific details like mobile detection and form factor determination through JavaScript interop.

The component uses a singleton pattern for global access and provides events for other parts of the system to react to XR state changes. This centralized management of platform and XR states allows for consistent behavior across different devices and modes in the ExeudVR environment.
