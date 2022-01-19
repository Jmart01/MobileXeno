using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    public static class MaterialFixer
    {
        [MenuItem("Assets/Material/Update To Standard")]
        private static void UpdateErrorMaterialToStandard()
        {
            string[] assets = AssetDatabase.FindAssets("t:Material");
            foreach(string asset in assets)
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(asset);
                Debug.Log(materialPath);
                //use the materialPath to find the material
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
                if(mat.shader == Shader.Find("Hidden/InternalErrorShader"))
                {
                    mat.shader = Shader.Find("Standard");
                }
            }
        }
    }
}
