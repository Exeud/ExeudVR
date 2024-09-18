# A Guide to Designing Interactions Using the ICVR Framework


## Overview

Both near (within reach) and far (line of sight) interactions are considered, as well as the transition from far to near. 

*Grip* button is used for near interaction and Jedi-grab*.

*Trigger* is used for distant manipulation and buttons, triggers etc.

>*Pressing *Grip* while holding an object with *Trigger* will first cause it to rotate then, after a short delay, with attract it to the hand, into a particular grip. Releasing *Grip* during the rotation phase will cancel the interaction.


## Interaction Design 

### Collisions and Layers




### Useful Modules

`RigidDynamics` is used to make object throwable. It uses the movement of the controller (mouse or hand) to give realistic release velocities and, if given a readable mesh, will calculate the volume of the object and, using the "density" parameter, will update the mass accordingly. Collisions are not affected.

When using `Wieldable` as a component, you can specify the relative position and orientation of the object when it is picked up with the Jedi-style mechanic (trigger -> grip). See Grabbable.md for instructions on how to set this position and rotation. Note than, even with this component, near interaction is unaffected.

The `ObjectInterface` is similar, but orients the hand to suit the object. This can identify interaction potential or remove ambiguity from interfaces. 

`ControlDynamics` allows objects to send event triggers on interaction with other objects, based on collision data. This is useful for levers, user interfaces or anywhere where second order effects are needed with user interaction.
