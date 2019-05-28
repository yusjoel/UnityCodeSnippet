﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CleanUpMaterials
{
    [MenuItem("Asset Tools/Clean Materials")]
    private static void _CleanProjectMaterials()
    {
        var paths =
            AssetDatabase.FindAssets("t:Material")
            .Select(AssetDatabase.GUIDToAssetPath);

        foreach (var path in paths)
        {
            CleanUnusedTextures(path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    }

    public static void CleanUnusedTextures(string materialPath)
    {
        var mat = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

        var so = new SerializedObject(mat);

        var shader = mat.shader;
        var activeTextureNames =
            Enumerable.Range(0, ShaderUtil.GetPropertyCount(shader))
            .Where(
                index =>
                ShaderUtil.GetPropertyType(shader, index)
                == ShaderUtil.ShaderPropertyType.TexEnv)
            .Select(index => ShaderUtil.GetPropertyName(shader, index));

        var activeTextureNameSet = new HashSet<string>(activeTextureNames);


        var texEnvsSp = so.FindProperty("m_SavedProperties.m_TexEnvs");
        for (var i = texEnvsSp.arraySize - 1; i >= 0; i--)
        {
            var texSp = texEnvsSp.GetArrayElementAtIndex(i);
            var texName = texSp.FindPropertyRelative("first").stringValue;
            if (!string.IsNullOrEmpty(texName))
            {
                if (!activeTextureNameSet.Contains(texName))
                {
                    texEnvsSp.DeleteArrayElementAtIndex(i);
                }
            }
        }

        so.ApplyModifiedProperties();
    }
}
