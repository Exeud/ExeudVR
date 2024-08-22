# Ex*euδ* VR Unity Toolkit 

Welcome to ExeudVR, helping you to build, host and share immersive experiences on the Internet Computer. This repository - a Unity toolkit - features modules that add dynamic physics, interaction, a rigged body and p2p multiplayer functionality to your Unity scene. Once built, it can be used with the [exeudvr-canister](https://github.com/willguest/exeudvr-canister) repository, to self-host these interactive experience on the bloackchain.

![317674693-e58d788d-b342-48f6-a6f9-e7550312ecdf](https://github.com/user-attachments/assets/b32d8473-7156-4d29-889f-b8266fd54508)

## 📦 Steps to create a new immersive experience

Make sure that [Unity 2020 LTS](https://unity.com/releases/editor/archive#download-archive-2020) is installed on your system.

1) Start a new URP project using [Unity Hub](https://unity.com/download).

2) Import the ExeudVR package, which can be downloaded from the [releases page](https://github.com/willguest/ExeudVR/releases). \
*Assets → Import Package → Custom Package*

3) Open the ExeudVR Setup window. \
*Window → WebXR → ExeudVR Setup*

4) Use the interface to complete the ExeudVR setup:

   - Switch build target to WebGL.
   - Add dependencies, using the relevant buttons.
   - Click on *Enable ExeudVR* to add enable the toolkit.
   - Apply relevant settings. Note that **this will override your current settings**.

5) Check that WebXR Export is enabled for the WebGL target. \
*Project Settings → XR Plug-in Management*

6) Open one of the starter scenes \
*Assets/ExeudVR/Scenes*

7) In the *Build Settings* window, add the current scene, then click *Build*

8) For compatibility with the [canister template](https://github.com/willguest/exeudvr-canister), put the build in a folder called `unity_build`.


## 🛠️ Technology Stack
- [Unity 2020 LTS](https://unity.com/releases/programmer-features/2020-lts-tier2-features): Immersive 3D development platform.
- [Universal Render Pipeline](https://unity.com/srp/universal-render-pipeline): A multiplatform render pipeline, suitable for WebXR.
- [WebXR Export](https://github.com/De-Panther/unity-webxr-export/): Core pipeline for rendering WebGL builds on the web.
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/): A modern, object-oriented programming language.


## 📄 Documentation

Each class is documented in a corresponding markdown file, and linked in the class definition summary. The [top level](https://github.com/willguest/ExeudVR/tree/develop/Documentation) contains instructions on how to add documentation to any contributions.


## 🌐 Self-Hosting

This repository is intended to be used with [exeudvr-canister](https://github.com/willguest/exeudvr-canister), which allows the Unity WebGL builds to be self-hosted on the Internet Computer blockchain. See that repository's README for more information.


## 💛 Sponsorship

The framework is open-source and largely funded by grants from the Internet Computer. I welcome sponsorship in all its forms and look forward to scaling this project as more resources become available. Please visit my [sponsorship page](https://github.com/sponsors/willguest) for more information.


## ⚖️ License

Code and documentation copyright 2023 Will Guest. Code released under the [Mozilla Public License 2.0](https://www.mozilla.org/en-US/MPL/2.0/FAQ/).
