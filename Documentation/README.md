#  Ex*euδ*VR Documentation

This is the entry point for the AI-supported documentation. The files and folders should reflect the code structure, to aid automatic updates and change monitoring, until an autonomous system is in place.


## Generating Documentation

The current strategy aims to be extremely easy to implement and seeks to enhance, rathan that interfere with the process of grokking the code.  Every significant class should have a corresponding markdown file in the documentation.


### Procedure for Generating Documentation 

1. Add a summary section to the top of the class definition. Write a short (1-2 sentence) summary. This is **important** and should be written by a human. It will be used by an LLM for semantic indexing, and should contain simple, descriptive statements containing keywords.

2. Give a capable LLM the following prompt:

```
LLM: This is a procedural algorithm for writing documentation with you, our trusty 'puter. Follow all steps described in each documentation file, especially where there is the signifier `LLM:` demonstrated at the start of this message.

This procedure writes documentation for the ExeudVR codebase. When giving responses, always be concise, use Markdown language, and use a step-wise approach. This is a big job, so it is okay to make plans and then go through them, step by step. You can request human confirmation, but try to be autonomous. Your tone should be professional, but not too formal, avoid repetition and words like 'crucial' and 'seamless'. No flim-flam.

INITIAL_PROCEDURE: The following steps are taken when generating documentation for ExeudVR:
    1) Read the template given in @Documentation\TEMPLATE.md
    2) Read and understand the code structure given in @Documentation\STRUCTURE.json
    3) Make an exhaustive list of the markdown items that will be checked, based on an interpretation of @Documentation\STRUCTURE.json
    4) Match EVERY ITEM in this list to create a complete list of C# files referenced by the markdown files, noticing all subfolders, by connecting each item after the following examples:
    `@Documentation\Controllers\BodyController.md -> @Assets\ExeudVR\Scripts\Controllers\BodyController.cs`, `@Documentation\Controllers\BodyRig\ExeudVRRig.md -> @Assets\ExeudVR\Scripts\Controllers\BodyRig\ExeudVRRig.cs`.
    5) Wait for instructions to add documentation for a specific file. When prompted, first read the code, taking note of any embedded comments, then output the documentation based on the contents of the C# file, while following @Documentation\TEMPLATE.md

Respond to this initial instruction by confirming whether both lists have been drawn up and mention whether they are of the same length (#). Whenever producing documentation, enclose it in ``` blocks for ease of use.
```

> Note: You may need to take additional actions for the TEMPLATE.md and STRUCTURE.json to be recognised as additional prompt context.

3. Once the LLM has confirmed that it is ready, simply write `document @script` indicating which file you wish to produce documentation for.

4. Copy the output into the relavant markdown file and save the file. 

5. Preview and/or check the file. For nested items, check that the link at the top takes you to the correct location. If there is an error, check the number of `../` in the path.

## Adding new classes

When adding a new C# class, the preferred style is to create a link between the C# code and the documentation, allowing the link to appear as a tooltip. This approach aims to save time and improve code readability. Feel free to comment the rest of the code as you wish.

To do this, add the following to the end of the XML summary, above the class definition:

```cs
<para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/<Folder>/<ClassName>.md"/>
```

Here's an example of an XML summary from the [RigidDynamics](https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Interaction/RigidDynamics.md) class:

```cs
/// <summary>
/// This component allows the object to be thrown with a force based on the hand velocity and weight of the object. Density will automatically update the object's mass and, if the mesh is readable, will calculate its volume. 
/// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Interaction/RigidDynamics.md"/>
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RigidDynamics : MonoBehaviour
{...
```

When producing documentation for the first time, also create a markdown file in the Documentation folder, at the location given by `STRUCTURE.json`, which is updated on Unity builds or can be triggered manually in Unity from the `Tools\Generate Documentation Structure` menu.
