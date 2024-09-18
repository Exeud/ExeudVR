/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


using UnityEngine;

namespace ExeudVR
{
    [System.Serializable]
    public class VRMap
    {
        public Transform rigTarget;
        public Vector3 trackingPositionOffset;
        public Vector3 trackingRotationOffset;

        public void Map(Transform vrTarget)
        {
            rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
            rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        }
    }

    /// <summary>
    /// Maps the body rig onto the ExeudVRCameraSet, using the BodyController as information conduit.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Controllers/BodyRig/ExeudVRRig.md"/>
    /// </summary>
    public class ExeudVRRig : MonoBehaviour
    {
        [Tooltip("Connection to ExeudVR \nLocation: ExeudVRCameraSet ➥ CharacterRoot ➥ Body")]
        [SerializeField] private BodyController bodyController;

        [SerializeField] private float yFac;

        public VRMap Body;
        public VRMap Head;
        public VRMap LeftHand;
        public VRMap RightHand;

        private Transform headRef;
        private Transform bodyRef;
        private Transform leftHandRef;
        private Transform rightHandRef;

        private void Start()
        {
            bodyController.Avatar = GetComponent<ExeudVRAvatarController>();

            headRef = bodyController.GetBodyReference("head");
            bodyRef = bodyController.GetBodyReference("body");
            leftHandRef = bodyController.GetBodyReference("leftHand");
            rightHandRef = bodyController.GetBodyReference("rightHand");
        }

        void Update()
        {
            Head.Map(headRef);
            Body.Map(bodyRef);
            RightHand.Map(rightHandRef);
            LeftHand.Map(leftHandRef);
        }
    }
}