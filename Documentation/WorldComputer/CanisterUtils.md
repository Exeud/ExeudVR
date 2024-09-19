# CanisterUtils
[CanisterUtils.cs](../../Assets/ExeudVR/Scripts/WorldComputer/CanisterUtils.cs)

## Description

Provides utility functions for interacting with the exeudvr-canister template, specifically UnityFunctions.ts.

## Public Variables, Functions, and Attributes

- `StartIIAuth` (int): Initiates the Internet Identity authentication process.
- `EndIISession` (int): Ends the current Internet Identity session.

## External Dependencies

- `ICLogin` (int): JavaScript function for logging in to Internet Computer.
- `ICLogout` (int): JavaScript function for logging out from Internet Computer.

## Methods

- `StartIIAuth` (int): Calls the ICLogin function in WebGL builds.
- `EndIISession` (int): Calls the ICLogout function in WebGL builds.

## How it Works

CanisterUtils serves as a bridge between Unity and the Internet Computer (IC) blockchain. It provides static methods to initiate authentication and end sessions with the IC. These methods are conditionally compiled to work only in WebGL builds, ensuring they're not called in the Unity editor or other platforms.
