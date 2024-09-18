/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

using System.Runtime.InteropServices;
using UnityEngine;
using WebXR;

namespace ExeudVR
{
    /// <summary>
    /// Cast from WebXRManager to speed up internal state checks,
    /// so order of `VR, AR, NORMAL` must be preserved.
    /// </summary>
    public enum XRState
    {
        VR,
        AR,
        NORMAL
    }

    /// <summary>
    /// This class looks after mode detection and switching, interfacing with the WebXRManager. 
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Managers/PlatformManager.md"/>
    /// </summary>
    public class PlatformManager : MonoBehaviour
    {
        private static PlatformManager _instance;
        public static PlatformManager Instance { get { return _instance; } }

        [DllImport("__Internal")]
        private static extern void DetectFormFactor(string objectName);

        public delegate void XRStateChange (XRState state);
        public event XRStateChange OnStateChange;


        public bool IsMobile
        {
            get => Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform;
            set { isMobile = value; }
        }

        public bool IsVRSupported { get; private set; }

        public XRState XrState { get; private set; }

        private bool discoveredVR = false;
        private bool isMobile;
        private string formFactor = "";

        

        public void StartVR()
        {
            discoveredVR = true;
        }

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

        void Start()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer &&
                WebXRManager.Instance.isSupportedVR)
            {
                IsVRSupported = true;
            }
            //DetectFormFactor(gameObject.name);
        }

        private void Update()
        {
            if (discoveredVR)
            {
                discoveredVR = false;
                WebXRManager.Instance.ToggleVR();
            }
        }

        private void OnEnable()
        {
            WebXRManager.OnXRChange += OnXRChange;
            WebXRManager.OnXRCapabilitiesUpdate += CheckCapabilties;
        }

        private void OnDisable()
        {
            WebXRManager.OnXRChange -= OnXRChange;
            WebXRManager.OnXRCapabilitiesUpdate -= CheckCapabilties;
        }

        
        private void CheckCapabilties(WebXRDisplayCapabilities capabilities)
        {
            IsVRSupported = capabilities.canPresentVR;
        }

        private void OnXRChange(WebXRState state, int viewsCount, Rect leftRect, Rect rightRect)
        {
            XrState = (XRState)state;
            OnStateChange(XrState);
            //DetectFormFactor(gameObject.name);
        }

        public void FormFactorResult(string formFactorResult)
        {
            if (!string.IsNullOrEmpty(formFactorResult))
            {
                Debug.Log("Form Factor: " + formFactorResult);
                formFactor = formFactorResult;
            }
        }

    }
}