# CanisterData
[CanisterData.cs](../../Assets/ExeudVR/Scripts/WorldComputer/CanisterData.cs)

## Description

Defines data structures for handling responses from Internet Computer canisters and authentication processes.

## Public Variables, Functions, and Attributes

- `AuthResponse`: Class representing the authentication response.
  - `cbIndex` (int): Callback index.
  - `result` (bool): Authentication result.
  - `principal` (string): Principal identifier.
  - `accountId` (string): Account identifier.

- `CallbackResponse`: Class for general callback responses.
  - `cbIndex` (int): Callback index.
  - `error` (string): Error message, if any.

- `CanisterResponse`: Class for canister responses.
  - `ErrorDetails` (CanisterResponseError): Detailed error information.

- `CanisterResponseError`: Class detailing canister error responses.
  - `RequestId` (string): Identifier for the request.
  - `ErrorCode` (string): Error code.
  - `RejectCode` (string): Rejection code.
  - `RejectMessage` (string): Detailed rejection message.

## How it Works

These data structures are crucial for handling communication between the ExeudVR application and Internet Computer canisters. They provide a structured way to process authentication results, callback responses, and error information from canister interactions.

The `Preserve` attribute is used throughout to ensure these classes are not stripped during build processes, maintaining their functionality in WebGL builds.

The `AuthResponse` class is particularly important for managing user authentication, storing essential information like the principal and account ID.

The `CanisterResponse` and `CanisterResponseError` classes offer a detailed error reporting structure, allowing the application to handle various error scenarios gracefully. The commented-out properties (`Canister` and `Method`) suggest potential for future expansion of error details.

These structures enable robust error handling and response processing in the ExeudVR system's interactions with Internet Computer canisters.
