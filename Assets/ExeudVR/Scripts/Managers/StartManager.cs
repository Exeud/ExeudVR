/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using UnityEngine;
using UnityEngine.Events;

namespace ExeudVR
{
    /// <summary>
    /// Links and handles events linked to stepping off the start pad.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Managers/StartManager.md"/>
    /// </summary>
    public class StartManager : MonoBehaviour
    {
        private static StartManager _instance;
        public static StartManager Instance { get { return _instance; } }

        [SerializeField] private UnityEvent OnInitialisation = new UnityEvent();

        private bool hasStarted = false;

        public delegate void InitialisationEvent();
        public event InitialisationEvent OnInitialised;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!hasStarted && other.attachedRigidbody.gameObject.GetComponentInChildren<Camera>())
            {
                hasStarted = true;
                OnInitialised?.Invoke();
                OnInitialisation?.Invoke();
            }
        }

    }
}