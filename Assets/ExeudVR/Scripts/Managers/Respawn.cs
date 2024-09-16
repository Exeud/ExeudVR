/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using ExeudVR.SharedAssets;

namespace ExeudVR
{
    /// <summary>
    /// Replaces object when they fall out of the scene and hit the 'Planes of Destruction'. 
    /// <see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Managers/Respawn.md"/>
    /// </summary>
    public class Respawn : MonoBehaviour
    {
        [SerializeField] private Transform DefaultRespawnPose;
        [SerializeField] private GameObject characterRoot;
        private Vector3 charStartPos;

        private void Awake()
        {
            charStartPos = characterRoot.transform.localPosition;
        }

        void OnTriggerEnter(Collider col)
        {
            GameObject respawnObject = col.gameObject;
            ManageRespawn(respawnObject);
        }

        private void ManageRespawn(GameObject respawnObject)
        {
            if (respawnObject.layer == LayerMask.NameToLayer("Body"))
            {
                ReplaceCharacter(respawnObject);
            }
            else if (!respawnObject.name.ToLower().Contains("hand")) // skip hands
            {
                ReplaceObject(respawnObject);
            }
        }

        private void ReplaceObject(GameObject obj)
        {
            string name = obj.name;
            Vector3 scale = obj.transform.localScale;

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            obj.TryGetComponent(out SharedAsset sa);
            obj.TryGetComponent(out RigidDynamics rd);

            // put shared and throwable assets back where they started
            if (sa)
            {
                obj.transform.position = sa.DefaultLocation;
                obj.transform.rotation = sa.DefaultRotation;
                obj.transform.localScale = sa.DefaultScale;
            }
            else if (rd)
            {
                obj.transform.position = rd.DefaultLocation;
                obj.transform.rotation = rd.DefaultRotation;
                obj.transform.localScale = rd.DefaultScale;
            }
            else
            {
                obj.transform.position = DefaultRespawnPose.position;
                obj.transform.rotation = DefaultRespawnPose.rotation;
                obj.transform.localScale = scale;
            }

            if (rb)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        private void ReplaceCharacter(GameObject specialObject)
        {
            characterRoot.GetComponent<Rigidbody>().velocity = Vector3.zero;
            characterRoot.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            characterRoot.transform.position = characterRoot.transform.parent.position + charStartPos;
            characterRoot.transform.rotation = Quaternion.identity;
            
        }
    }
}