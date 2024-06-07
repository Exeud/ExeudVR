# Exeud Virtual Reality 

Welcome to the ExeudVR, a Unity-based toolkit for designing cutting-edge immersive experiences on the Internet Computer.


## 📦 Steps to Create an ExeudVR Experience

Make sure that [Unity 2020 LTS](https://unity.com/releases/editor/archive#download-archive-2020) is installed on your system.

1) Start a new URP project using [Unity Hub](https://unity.com/download).

2) Import the ExeudVR package. \
*Assets → Import Package → Custom Package*

3) Open the ICVR Setup window. \
*Window → WebXR → ExeudVR Setup*

4) Use the interface to complete the ExeudVR setup:

   - Switch build target to WebGL.
   - Add dependencies, using the relevant buttons.
   - Click on *Enable ExeudVR* to add the `ExeudVR` Scripting Define Symbol.
   - Apply relevant settings. Note that **this will override your current settings**.

5) Check that WebXR Export is enabled for the WebGL target. \
*Project Settings → XR Plug-in Management*

6) Open one of the test scenes \
Bare bones: *Assets/ExeudVR/Scenes* \
(https://github.com/willguest/ExeudVR/releases)

7) Open the *Build Settings* window, add the current scene and click *Build*

8) For compatibility with the [exeud-canister](https://github.com/willguest/exeud-canister) template, place the build in a folder called `unity_build`.


## 🛠️ Technology Stack
- [Unity 2020 LTS](https://unity.com/releases/programmer-features/2020-lts-tier2-features): Immersive 3D development platform.
- [Universal Render Pipeline](https://unity.com/srp/universal-render-pipeline): A multiplatform render pipeline, suitable for WebXR.
- [WebXR Export](https://github.com/De-Panther/unity-webxr-export/): Core pipeline for rendering WebGL builds on the web.
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/): A modern, object-oriented programming language.


## 📄 Documentation

Each class is documented in a corresponding markdown file, and linked in the class definition summary. The [top level](https://github.com/willguest/ExeudVR/tree/develop/Documentation) contains instructions on how to add documentation to any contributions.

## 🌐 Self-Hosting

This repository is intended to be used with [exeud-canister](https://github.com/willguest/exeud-canister), which allows the Unity WebGL builds to be self-hosted on the Internet Computer blockchain. See that repository's README for more information.


## 💛 Sponsorship

The framework is open-source and was initially funded by non-dilutive grants from the Internet Computer. I welcome sponsorship in all forms and look forward to scaling this project as more resources become available. Please visit my [sponsorship page](https://github.com/sponsors/willguest) for more information.


## ⚖️ License

Code and documentation copyright 2023 Will Guest. Code released under the [Mozilla Public License 2.0](https://www.mozilla.org/en-US/MPL/2.0/FAQ/).
