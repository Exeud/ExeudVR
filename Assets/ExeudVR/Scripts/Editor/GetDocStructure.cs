/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 */


#if UNITY_EDITOR && UNITY_WEBGL
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System.Text;
using System.Linq;


namespace ExeudVR
{
    /// <summary>
    /// Generates a json file, in the documentation root, that contains all scripts in the ExeudVR namespace.
    /// <para /><see href="https://github.com/Exeud/ExeudVR/tree/develop/Documentation/Editor/GetDocStructure.md"/>
    /// </summary>
    public class GetDocStructure : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        [MenuItem("Tools/Generate Documentation Structure")]
        public static void GenerateStructureManually()
        {
            var instance = new GetDocStructure();
            instance.GenerateStructure();
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            GenerateStructure();
        }

        void GenerateStructure()
        {
            string rootPath = "Assets/ExeudVR/Scripts";
            string projectRoot = Directory.GetParent(UnityEngine.Application.dataPath).FullName;
            string outputPath = Path.Combine(projectRoot, "Documentation", "STRUCTURE.json");

            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            BuildJsonStructure(rootPath, json, 2);
            json.AppendLine("}");

            File.WriteAllText(outputPath, json.ToString());
            AssetDatabase.Refresh();
        }

        void BuildJsonStructure(string path, StringBuilder json, int indent)
        {
            string[] directories = Directory.GetDirectories(path);

            string[] csFiles = Directory.GetFiles(path, "*.cs")
                .Where(file => IsInExeudVRNamespace(file))
                .ToArray();

            json.AppendLine($"{new string(' ', indent)}\"{Path.GetFileName(path.Replace("Scripts", "Documentation"))}\": {{");

            for (int i = 0; i < directories.Length; i++)
            {
                BuildJsonStructure(directories[i], json, indent + 2);
                if (i < directories.Length - 1 || csFiles.Length > 1)
                    json.AppendLine(",");
            }

            if (csFiles.Length > 0)
            {
                if (directories.Length > 1)
                    json.AppendLine(",");
                json.AppendLine($"{new string(' ', indent + 2)}\"files\": [");
                for (int i = 0; i < csFiles.Length; i++)
                {
                    string className = Path.GetFileNameWithoutExtension(csFiles[i]) + ".md";
                    json.Append($"{new string(' ', indent + 4)}\"{className}\"");
                    if (i < csFiles.Length - 1)
                        json.AppendLine(",");
                    else
                        json.AppendLine();
                }
                json.AppendLine($"{new string(' ', indent + 2)}]");
            }

            json.Append($"{new string(' ', indent)}}}");
        }

        bool IsInExeudVRNamespace(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                for (int i = 0; i < 20; i++) // Read first 20 lines
                {
                    string line = reader.ReadLine();
                    if (line == null) break;
                    if (line.Contains("namespace ExeudVR"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
#endif