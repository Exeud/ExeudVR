# ChainUtils
[ChainUtils.cs](../../Assets/ExeudVR/Scripts/WorldComputer/ChainUtils.cs)

## Description

Provides utility functions for interrogating and processing canister responses in the ExeudVR system.

## Public Variables, Functions, and Attributes

- `InterrogateCanisterResponse` (string): Static method to analyze and log details of canister responses.

## Methods

- `InterrogateCanisterResponse` (string): 
  - Parses the JSON response from a canister.
  - Logs error codes and reject messages if present.
  - Handles exceptions during the parsing process.

## How it Works

Thisclass is designed to provide detailed insights into canister responses, particularly when errors occur.

The `InterrogateCanisterResponse` method takes a JSON string as input, which is expected to be the raw response from an IC canister. It uses Newtonsoft.Json to deserialize this response into a `CanisterResponseError` object.

The method then performs a series of checks:
1. It first verifies if the deserialization was successful.
2. If an error code is present, it logs this code.
3. If a reject message is present, it logs both the reject code and message.

The use of Unity's Debug.Log and Debug.LogError ensures that these messages are easily visible in the Unity console, facilitating efficient debugging in both development and production environments.