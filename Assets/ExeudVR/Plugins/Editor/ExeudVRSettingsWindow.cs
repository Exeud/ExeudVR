#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace ExeudVR.Settings
{
    /// <summary>
    /// The main class responsible for the ExeudVR Setup Window.
    /// 
    /// </summary>
    public class ExeudVRSettingsWindow : EditorWindow
    {
        private ExeudVRSettingsData settingsData;
        private ExeudVRProjectSettings projectSettings;

        private static string PRESET_PATH = "Assets/ExeudVR/Settings/Presets";

        private static string LIGHT_DATA_ASSET = Path.Combine(PRESET_PATH, "ExeudVR_LightingSettings.preset");
        private static string LIGHT_SETTINGS = Path.Combine(PRESET_PATH, "ExeudVR_Lighting.lighting");

        private static string TAG_MNGR_ASSET = "ProjectSettings/TagManager.asset";
        private static string PHYS_MNGR_ASSET = "ProjectSettings/DynamicsManager.asset";
        private static string GRAPH_MNGR_ASSET = "ProjectSettings/GraphicsSettings.asset";
        private static string QUAL_MNGR_ASSET = "ProjectSettings/QualitySettings.asset";
        private static string PLAY_MNGR_ASSET = "ProjectSettings/ProjectSettings.asset";
        private static string EDTR_MNGR_ASSET = "ProjectSettings/EditorSettings.asset";


        [MenuItem("Window/WebXR/ExeudVR Setup")]
        public static void ShowWindow()
        {
            GetWindow<ExeudVRSettingsWindow>("ExeudVR Setup");
        }

        private Dictionary<string, bool> packageStatus;
        private KeyValuePair<string, bool>[] presets;

        private bool isWebGL;
        private bool hasDataAsset = false;
        private bool hasDependencies = false;

        private string[] presetFiles;
        private bool[] presetStates;

        private void CreateGUI()
        {
            isWebGL = EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL;
            packageStatus = new Dictionary<string, bool>();

            // Create settings objects
            projectSettings = CreateInstance<ExeudVRProjectSettings>();
            projectSettings.GetProjectPackages();

            // Check dependency status
            hasDependencies = HasDependencies(false);

            // Get the properties of the .preset files
            presetFiles = Directory.GetFiles(PRESET_PATH, "*.preset");
            presetStates = new bool[presetFiles.Length];
            presets = new KeyValuePair<string, bool>[presetFiles.Length];

            // Start the settings data object
            settingsData = ExeudVRSettingsData.instance;
            hasDataAsset = settingsData.Initialise(presetFiles.Length);

            if (hasDataAsset)
            {
                for (int f = 0; f < presetFiles.Length; f++)
                {
                    var assetData = settingsData.ExeudVRSettings[f];
                    if (assetData.FilePath == presetFiles[f])
                    {
                        presets[f] = new KeyValuePair<string, bool>(assetData.FileName, assetData.PresetState);
                    }
                    else
                    {
                        presets[f] = new KeyValuePair<string, bool>(presetFiles[f], false);
                    }
                }
            }
        }

        private bool HasDependencies(bool useAsync = true)
        {
            bool one = CheckPackagePresence("com.unity.nuget.newtonsoft-json", useAsync);
            bool two = CheckPackagePresence("com.de-panther.webxr", useAsync);
            bool three = CheckPackagePresence("com.unity.animation.rigging", useAsync);
            return (one && two && three);
        }

        private void OnGUI()
        {
            var titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.normal.textColor = Color.white;
            titleStyle.fontStyle = FontStyle.Bold;

            var refStyle = new GUIStyle(GUI.skin.label);
            refStyle.normal.textColor = Color.white;
            refStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Build Target", titleStyle);
            EditorGUI.BeginDisabledGroup(isWebGL);
            if (GUILayout.Button("Set WebGL"))
            {
                EnsureWebGLBuildTarget();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space(15, true);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Dependencies", titleStyle, GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            EditorGUI.BeginDisabledGroup(hasDependencies);
            if (GUILayout.Button("↻", refStyle, GUILayout.Width(20)))
            {
                hasDependencies = HasDependencies();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            DisplayPackage("Newtonsoft Json", "com.unity.nuget.newtonsoft-json");
            DisplayPackage("Animation Rigging", "com.unity.animation.rigging");
            DisplayPackage("WebXR Export", "com.de-panther.webxr");

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(15, true);

            GUILayout.Label("\nIMPORTANT: Changes to settings are irreversible. " +
                        "When applied, modify preset files to control associated settings. \n" +
                        "➤ " + PRESET_PATH + '\n', EditorStyles.wordWrappedLabel);

            EditorGUI.BeginDisabledGroup(!hasDependencies || SDSUtility.ContainsSymbol(BuildTargetGroup.WebGL, "ExeudVR"));
            if (GUILayout.Button("Enable ExeudVR")) 
            {
                SDSUtility.AddSymbol(BuildTargetGroup.WebGL, "ExeudVR");
            }
            EditorGUI.EndDisabledGroup();

            // disable settings until deps are in
            bool depsAreIn = SDSUtility.ContainsSymbol(BuildTargetGroup.WebGL, "ExeudVR");
            EditorGUI.BeginDisabledGroup(!depsAreIn);

            EditorGUILayout.Space(15, true);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Settings", titleStyle);

            if (presets == null) return;

            // Set the default state of each preset to false (off)
            for (int i = 0; i < presets.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(presetFiles[i]);
                string shortName = filename.Substring(5, filename.Length - 5);
                presetStates[i] = presets[i].Value;

                GUILayout.BeginHorizontal();

                GUILayout.Label(shortName);
                GUILayout.FlexibleSpace();

                if (!presets[i].Value)
                {
                    if (GUILayout.Button("Apply", GUILayout.Width(50)))
                    {
                        presets.SetValue(new KeyValuePair<string, bool>(filename, true), i);

                        // Update settings
                        string filepath = presetFiles[i];
                        string manager = IdentifyManager(filename);
                        if (!string.IsNullOrEmpty(manager))
                        {
                            UpdateSettings(filepath, manager);
                        }
                    }
                }
                else
                {
                    var greenStyle = new GUIStyle(GUI.skin.label);
                    greenStyle.normal.textColor = Color.green;
                    EditorGUILayout.LabelField("✔", greenStyle, GUILayout.MaxWidth(20));

                    if (GUILayout.Button("Cancel", GUILayout.Width(60)))
                    {
                        presets.SetValue(new KeyValuePair<string, bool>(filename, false), i);
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Apply All"))
            {
                bool isConfirmed = EditorUtility.DisplayDialog("Confirmation",
                    "Are you sure you want to apply all presets?", "OK", "Cancel");

                if (isConfirmed)
                {
                    for (int i = 0; i < presets.Length; i++)
                    {
                        if (!presetStates[i])
                        {
                            string filename = Path.GetFileNameWithoutExtension(presetFiles[i]);
                            presetStates[i] = true;

                            presets.SetValue(new KeyValuePair<string, bool>(filename, true), i);
                            settingsData.ModifyDataAsset(presets.Length, filename, presetStates[i]);

                            string filepath = presetFiles[i];
                            string manager = IdentifyManager(filename);
                            if (!string.IsNullOrEmpty(manager))
                            {
                                UpdateSettings(filepath, manager);
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUI.EndDisabledGroup();
        }

        void DisplayPackage(string label, string packageName)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(120));
            GUILayout.FlexibleSpace();

            if (packageStatus == null)
            {
                packageStatus = new Dictionary<string, bool>();
            }

            if (packageStatus.TryGetValue(packageName, out bool isPresent))
            {
                if (packageStatus[packageName])
                {
                    var greenStyle = new GUIStyle(GUI.skin.label);
                    greenStyle.normal.textColor = Color.green;
                    EditorGUILayout.LabelField("✔", greenStyle, GUILayout.MaxWidth(20));
                }
            }

            EditorGUI.BeginDisabledGroup(isPresent);
            if (GUILayout.Button("Install", GUILayout.Width(50)))
            {
                projectSettings.TryIncludePackage(packageName);
                hasDependencies = HasDependencies(false);

            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        private bool CheckPackagePresence(string packageName, bool useAsync = true)
        {
            bool isPresent = useAsync ?
                projectSettings.CheckForPackage(packageName) :
                projectSettings.CheckPackSync(packageName);

            if (packageStatus.ContainsKey(packageName))
            {
                packageStatus[packageName] = isPresent;
            }
            else
            {
                packageStatus.Add(packageName, isPresent);
            }
            return isPresent;
        }

        public static void EnsureWebGLBuildTarget()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
            }
        }

        private string IdentifyManager(string presetName)
        {
            switch (presetName)
            {
                case "ExeudVR_Tags":
                    return TAG_MNGR_ASSET;
                case "ExeudVR_Physics":
                    return PHYS_MNGR_ASSET;
                case "ExeudVR_Graphics":
                    return GRAPH_MNGR_ASSET;
                case "ExeudVR_Quality":
                    return QUAL_MNGR_ASSET;
                case "ExeudVR_PlayerSettings":
                    return PLAY_MNGR_ASSET;
                case "ExeudVR_EditorSettings":
                    return EDTR_MNGR_ASSET;
                case "ExeudVR_LightingSettings":
                    UpdateLighting();
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        private void UpdateLighting()
        {
            var lightingDataAsset = AssetDatabase.LoadMainAssetAtPath(LIGHT_DATA_ASSET) as Preset;
            LightingSettings lightingSettings = AssetDatabase.LoadMainAssetAtPath(LIGHT_SETTINGS) as LightingSettings;
            SerializedObject lightManager = new SerializedObject(lightingSettings);

            try
            {
                lightingDataAsset.ApplyTo(lightManager.targetObject);

                Lightmapping.SetLightingSettingsForScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene(), lightingSettings);
            }
            catch (Exception e)
            {
                Debug.Log("Lighting Error: " + e.ToString());
            };

            lightManager.ApplyModifiedProperties();
            lightManager.UpdateIfRequiredOrScript();
            Lightmapping.BakeAsync();
        }

        private static void UpdateSettings(string preset, string manager)
        {
            Preset settingsPreset = AssetDatabase.LoadMainAssetAtPath(preset) as Preset;
            SerializedObject settingsManager = new SerializedObject(AssetDatabase.LoadMainAssetAtPath(manager));
            settingsPreset.ApplyTo(settingsManager.targetObject);
            settingsManager.ApplyModifiedProperties();
            settingsManager.Update();
        }

        private void UpdateWebXRSettings(string preset, string manager)
        {
            
        }

    }
}
#endif