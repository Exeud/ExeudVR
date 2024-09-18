/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using WebXR;
using UnityEditor.XR.Management;
using UnityEngine.XR.Management;

namespace ExeudVR
{
    /// <summary>
    /// Updates settings from the Setup Window. Also hard-codes WebXR settings.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Editor/EditorSettingsManager.md"/>
    /// </summary>
    [InitializeOnLoad]
    public class EditorSettingsManager : ScriptableObject
    {
        static EditorSettingsManager()
        {
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }

        void OnDestroy()
        {
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }

        static void OnAfterAssemblyReload()
        {
            TryUpdateWebXRSettings(true); 
        }

        /// <summary>
        /// Used to override the WebXR Project Settings for compatibility with ExeudVRCameraSet. 
        /// The use of 'SessionState' allows the value to survive assembly reloads, but is 
        /// cleared when Unity exits.
        /// </summary> 
        public static void TryUpdateWebXRSettings(bool forceUpdate = false)
        {
            if (!SessionState.GetBool("hasUpdatedSettings", false) || forceUpdate)
            {
                SessionState.SetBool("hasUpdatedSettings", true);

                // set 'Initialize XR on Startup' to true
                XRGeneralSettingsPerBuildTarget buildTargetSettings = null;
                EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out buildTargetSettings);

                if (buildTargetSettings)
                {
                    XRGeneralSettings settings = buildTargetSettings.SettingsForBuildTarget(BuildTargetGroup.WebGL);
                    settings.InitManagerOnStart = true;
                }
                else
                {
                    SessionState.SetBool("hasUpdatedSettings", false); 
                }

                // TODO: assign loader and enable WebXR from here.
                // See https://forum.unity.com/threads/editor-programmatically-set-the-vr-system-in-xr-plugin-management.972285/

                // set WebXR plugin settings
                try
                {
                    EditorBuildSettings.TryGetConfigObject("WebXR.Settings", out WebXRSettings webXRSettings);

                    // Hard-code WebXR Settings
                    webXRSettings.VRRequiredReferenceSpace = WebXRSettings.ReferenceSpaceTypes.local;
                    webXRSettings.VROptionalFeatures = 0;
                    webXRSettings.ARRequiredReferenceSpace = WebXRSettings.ReferenceSpaceTypes.local;
                    webXRSettings.AROptionalFeatures = 0;

                    webXRSettings.UseFramebufferScaleFactor = false;
                    webXRSettings.UseNativeResolution = false;
                    webXRSettings.FramebufferScaleFactor = 1;

                    webXRSettings.AutoLoadWebXRManager = false;
                    webXRSettings.AutoLoadWebXRInputSystem = false;
                    webXRSettings.DisableXRDisplaySubsystem = false;

                    EditorUtility.SetDirty(webXRSettings);
                    AssetDatabase.SaveAssetIfDirty(webXRSettings);

                    Debug.Log("WebXR settings updated");
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
#endif