/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ExeudVR
{
    [System.Serializable]
    public class GameObjectFloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class GameObjectBoolEvent : UnityEvent<bool> { }

    /// <summary>
    /// A simplified version of 'Grabbable', for single handed interaction only. For more information 
    /// <see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Interaction/ObjectInterface.md"/>
    /// </summary>
    public class ObjectInterface : MonoBehaviour
    {
        [SerializeField] private Transform controlPoseLeft;
        [SerializeField] private Transform controlPoseRight;
        [SerializeField] private string gripPose;

        [SerializeField] private UnityEvent OnGetFocusEvent;
        [SerializeField] private UnityEvent OnLoseFocusEvent;

        [SerializeField] private UnityEvent<bool> OnGripEvent;
        [SerializeField] private UnityEvent<float> OnTriggerEvent;

        private Transform previousParent;
        private GameObject currentManipulator;
        private ControllerHand activeHand;

        private float triggerEnterTick = 0f;
        private float triggerExitTick = 0f;

        private bool IsBeingUsed;
        private bool IsBeingHeld;

        public void ToggleActivation(GameObject manipulator, bool state)
        {
            if (!manipulator || manipulator.GetComponent<CursorManager>())
            {
                if (state)
                {
                    IsBeingUsed = state;
                    OnGetFocusEvent?.Invoke();
                }
                else
                {
                    OnLoseFocusEvent?.Invoke();
                    IsBeingUsed = state;
                }
            }
            else
            {
                if (state)
                {
                    if (ReceiveControl(manipulator))
                    {
                        IsBeingUsed = state;
                        OnGetFocusEvent?.Invoke();
                    }
                }
                else
                {
                    if (LoseControl())
                    {
                        OnLoseFocusEvent?.Invoke();
                        IsBeingUsed = state;
                    }
                }
            }
        }

        public void SetTrigger(float triggerValue)
        {
            if (IsBeingUsed || IsBeingHeld)
            {
                OnTriggerEvent?.Invoke(triggerValue);
            }
        }

        public void SetGrip(bool state)
        {
            IsBeingHeld = state;
            OnGripEvent?.Invoke(state);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Time.realtimeSinceStartup - triggerEnterTick < 0.1f) return;
            if (!IsBeingUsed && other.gameObject.GetComponent<XRController>())
            {
                triggerEnterTick = Time.realtimeSinceStartup;
                ToggleActivation(other.gameObject, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Time.realtimeSinceStartup - triggerExitTick < 0.1f) return;
            if (IsBeingUsed && other.gameObject.GetComponent<XRController>())
            {
                triggerExitTick = Time.realtimeSinceStartup;
                ToggleActivation(other.gameObject, false);
            }
        }

        private bool ReceiveControl(GameObject manipulator)
        {
            if (manipulator == null) return false;

            // hand-based control
            if (manipulator && manipulator.TryGetComponent(out XRController xrctrl))
            {
                // compatibility checks
                if (xrctrl.IsUsingInterface) return false;
                if (xrctrl.IsControllingObject) return false;

                activeHand = xrctrl.SetCurrentInterface(true, this);

                Transform activeControlPose;
                if (activeHand == ControllerHand.LEFT && controlPoseLeft != null)
                {
                    activeControlPose = controlPoseLeft;
                }
                else if (activeHand == ControllerHand.RIGHT && controlPoseRight != null)
                {
                    activeControlPose = controlPoseRight;
                }
                else
                {
                    return false;
                }

                previousParent = manipulator.transform;

                // send grip update, if it exists
                if (!string.IsNullOrEmpty(gripPose))
                {
                    xrctrl.SetGripPose(gripPose);
                }

                
                currentManipulator = xrctrl.HandModel;

                // disable hand colliders
                foreach (CapsuleCollider cc in currentManipulator.GetComponentsInChildren<CapsuleCollider>())
                {
                    cc.enabled = false;
                }

                StartCoroutine(LerpToControlPose(currentManipulator, transform,
                    activeControlPose.localPosition, activeControlPose.localRotation, 0.2f));
            }
            else
            {
                currentManipulator = manipulator;
            }
            return true;
        }

        private bool LoseControl()
        {
            if (currentManipulator == null) return false;

            if (IsBeingHeld) return false;

            if (gameObject.TryGetComponent(out ControlDynamics cd))
            {
                cd.ResetPose();
            }

            if (previousParent.TryGetComponent(out XRController xrc))
            {
                xrc.SetGripPose("relax");
                activeHand = xrc.SetCurrentInterface(false, null);

                StartCoroutine(LerpToControlPose(currentManipulator, previousParent,
                    Vector3.zero, Quaternion.identity, 0.2f));

                // re-enable hand colliders
                foreach (CapsuleCollider cc in currentManipulator.GetComponentsInChildren<CapsuleCollider>())
                {
                    cc.enabled = true;
                }
            }
            currentManipulator = null;
            return true;
        }

        private IEnumerator LerpToControlPose(GameObject objToLerp, Transform newParent, Vector3 endPosition, Quaternion endRotation, float duration)
        {
            // switch parent
            objToLerp.transform.parent = newParent;

            if (endPosition == Vector3.zero)
            {
                objToLerp.transform.localScale = Vector3.one;
            }

            yield return new WaitForEndOfFrame();

            Transform t = objToLerp.transform;
            float time = 0;
            while (time < duration)
            {
                objToLerp.transform.localPosition = Vector3.Lerp(t.localPosition, endPosition, time / duration);
                objToLerp.transform.localRotation = Quaternion.Slerp(t.localRotation, endRotation, time / duration);

                time += Time.smoothDeltaTime;
                yield return null;
            }

            objToLerp.transform.localPosition = endPosition;
            objToLerp.transform.localRotation = endRotation;
        }
    }
}