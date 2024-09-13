# Ex*euδ*VR Unity Toolkit 

Welcome to ExeudVR, a toolkit to help you to build, host and share immersive experiences on the Internet Computer.

This repository contains modules that add dynamic physics, easy configuration for object interaction, a rigged body, and WebRTC multiplayer functionality to a Unity scene. Once built, it can be used with the [exeudvr-canister](https://github.com/willguest/exeudvr-canister) template, to self-host these interactive experience on the blockchain.

![317674693-e58d788d-b342-48f6-a6f9-e7550312ecdf](https://github.com/user-attachments/assets/b32d8473-7156-4d29-889f-b8266fd54508)

## 🏗️ Steps to create a new immersive experience

Make sure that [Unity 2020 LTS](https://unity.com/releases/editor/whats-new/2020.3.48#installs) is installed on your system.

1) Open [Unity Hub](https://unity.com/download) and start a new project using the '3D Sample Scene (URP)' template.

2) Import the ExeudVR package, which can be downloaded from the [releases page](https://github.com/willguest/ExeudVR/releases). <br>
*Assets → Import Package → Custom Package*

<img src="https://github.com/user-attachments/assets/4dbeb966-147e-4033-82f0-83a32faad2a3" align="right" width="200px" height="391px"/>

3) Open the ExeudVR Setup window (pictured).<br>
*Window → WebXR → ExeudVR Setup*

4) Use the interface to complete the setup process: <br>
   - Switch build target to WebGL.
   - Add dependencies, using the relevant buttons.
   - Tap `Enable ExeudVR` to add enable the toolkit.
   - Apply settings. Note: **this will override your current settings**.

5) Check that WebXR Export is enabled for the WebGL target. <br>
*Project Settings → XR Plug-in Management*

6) Open one of the starter scenes, and press ▶️ to try it out. <br>
*Assets/ExeudVR/Scenes*

7) When ready, open the *Build Settings* window, <br>
add the current scene, and press `Build`.


## 🛠️ Technology Stack
- [Unity 2020 LTS](https://unity.com/releases/programmer-features/2020-lts-tier2-features): Immersive 3D development platform.
- [Universal Render Pipeline](https://unity.com/srp/universal-render-pipeline): A multiplatform render pipeline, suitable for WebXR.
- [WebXR Export](https://github.com/De-Panther/unity-webxr-export/): Core pipeline for rendering WebGL builds on the web.
- [C#](https://learn.microsoft.com/en-us/dotnet/csharp/): A modern, object-oriented programming language.


## 📚 Documentation

Each class is documented in a corresponding markdown file, and linked in the class definition summary. The [top level](https://github.com/Exeud/ExeudVR/tree/develop/Documentation) contains instructions on how to add documentation to any contributions.


## 👑 Self-Hosting

This repository is intended to be used with [exeudvr-canister](https://github.com/willguest/exeudvr-canister), which allows Unity WebGL builds to be self-hosted on the Internet Computer blockchain. See that repository's README for more information and a step-by-step guide to putting your experience on-chain.


## 👀 More Information

Visit our [homepage](https://exeud.com) for more information, featured experiences built with Ex*euδ*VR, details of the services we offer, and contact information. We are open to collaborations of all kinds, and welcome suggestions, ideas and opinions that can help us improve the tools we make.

If you would like to become a contributor to this repository, or the canister template, please reach out and we will share contribution guidelines and other useful information.


## ⚖️ License

All code is released under the [Mozilla Public License 2.0](https://www.mozilla.org/en-US/MPL/2.0/FAQ/), unless otherwise stated in the file itself, and also applies to any files in this repository that do not yet contain the relevant header. When combining code from this repository with your own, MPL 2.0 places no restrictions on the way other files in the project are licensed. In short, MPL 2.0 says that, if you modify and distribute any files, you are obliged to open-source those files, but not any others in your project. Deploying VR experience on the web does not count as distribution.
