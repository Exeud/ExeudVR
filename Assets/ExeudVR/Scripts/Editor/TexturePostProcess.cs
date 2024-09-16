using UnityEngine.Rendering.Universal;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR && UNITY_WEBGL

using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Rendering;

public class TexturePostProcess : IPreprocessBuildWithReport, IPreprocessShaders
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report) => CleanTextures();


    void CleanTextures()
    {
        ScriptableRendererData[] rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(UniversalRenderPipeline.asset);

        for (int i = 0; i < rendererDataList.Length; i++)
        {
            ForwardRendererData frd = (ForwardRendererData)rendererDataList[i];
            if (frd != null)
            {
                var noTexBef = frd.postProcessData.textures.filmGrainTex.Length;
                Debug.Log("Found postProcessor '" + rendererDataList[i].name + "'. Removing " + noTexBef + " film grain textures");

                frd.postProcessData.textures = null;
            }
        }
    }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> shaderCompilerData)
    {
        if (shader.name.Contains("UberPost"))
        {
            shaderCompilerData.Clear();
        }
    }

}
#endif
