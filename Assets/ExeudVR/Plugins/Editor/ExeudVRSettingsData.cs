#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace ExeudVR.Settings
{
    [System.Serializable]
    public class ScopedRegistry
    {
        public string name;
        public string url;
        public string[] scopes;
    }

    [System.Serializable]
    public class ManifestJson
    {
        public Dictionary<string, string> dependencies = new Dictionary<string, string>();
        public List<ScopedRegistry> scopedRegistries = new List<ScopedRegistry>();
    }

    [System.Serializable]
    public class ExeudVRSettingsObject
    {
        public string Id;
        public string FileName = string.Empty;
        public string FilePath = string.Empty;
        public bool PresetState = false;
    }

    /// <summary>
    /// A generator and handler for the named data asset, which is constructed from the 
    /// preset files held in the subfolder. Not currently viewable in the inspector.
    /// </summary>
    [System.Serializable, FilePath("Assets/ExeudVR/Settings/ExeudVRSettingsData.asset", FilePathAttribute.Location.ProjectFolder)]
    public class ExeudVRSettingsData : ScriptableSingleton<ExeudVRSettingsData>
    {
        public string FilePath { get { return GetFilePath(); } }

        [SerializeField]
        public List<ExeudVRSettingsObject> ExeudVRSettings;

        public bool Initialise(int chkLen)
        {
            if (instance == null)
            {
                return false;
            }
            else
            {
                if (ExeudVRSettings == null || chkLen != ExeudVRSettings.Count)
                {
                    Debug.Log("ExeudVRSettingsData.asset missing or changed, rebuilding...");
                    MakeNewDataAsset();
                }
                return (chkLen == instance.ExeudVRSettings.Count);
            }
        }

        public void ModifyDataAsset(int chkLen, string fn, bool pstate)
        {
            List<ExeudVRSettingsObject> buffer = ExeudVRSettings.GetRange(0, ExeudVRSettings.Count);
            ExeudVRSettings.Clear();

            for (int i = 0; i < buffer.Count; i++)
            {
                bool marked = false;
                if (buffer[i].FileName == fn)
                {
                    marked = true;
                }

                var vrso = new ExeudVRSettingsObject
                {
                    Id = buffer[i].Id,
                    FileName = buffer[i].FileName,
                    FilePath = buffer[i].FilePath,
                    PresetState = marked ? pstate : buffer[i].PresetState
                };
                ExeudVRSettings.Add(vrso);
            }

            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssetIfDirty(instance);
            Save(true);
        }

        private void MakeNewDataAsset() 
        {
            ExeudVRSettings = new List<ExeudVRSettingsObject>();

            // Get the path to the folder containing the .preset files
            string folderPath = "Assets/ExeudVR/Settings/Presets";

            // Get the names of the .preset files in the folder
            string[] presetFiles = Directory.GetFiles(folderPath, "*.preset");

            // Set the default state of each preset to false (off)
            for (int i = 0; i < presetFiles.Length; i++)
            {
                string name = Path.GetFileNameWithoutExtension(presetFiles[i]);
                string path = presetFiles[i];

                var vrso = new ExeudVRSettingsObject
                {
                    Id = AssetDatabase.AssetPathToGUID(path),
                    FileName = name,
                    FilePath = path,
                    PresetState = false
                };

                ExeudVRSettings.Add(vrso);
            }

            EditorUtility.SetDirty(instance);
            AssetDatabase.SaveAssetIfDirty(instance);
            Save(true);
            AssetDatabase.Refresh();
        }

    }
}
#endif