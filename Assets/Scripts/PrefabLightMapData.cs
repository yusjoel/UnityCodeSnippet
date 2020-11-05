using System;
using UnityEditor;
using UnityEngine;

public class PrefabLightMapData : MonoBehaviour
{
    public Texture2D LightmapDir;
    public Texture2D LightmapLight;
    [SerializeField]
    public LightMapInfo[] LightMapInfos;

    void Start()
    {
        LightmapSettings.lightmapsMode = LightmapsMode.CombinedDirectional;

        var lightmaps = new LightmapData[1];
        lightmaps[0] = new LightmapData {lightmapColor = LightmapLight, lightmapDir = LightmapDir};
        LightmapSettings.lightmaps = lightmaps;

        foreach (var lightMapInfo in LightMapInfos)
        {
            lightMapInfo.Renderer.lightmapIndex = lightMapInfo.LightmapIndex;
            lightMapInfo.Renderer.lightmapScaleOffset = lightMapInfo.LightmapScaleOffset;
        }

        StaticBatchingUtility.Combine(gameObject);
    }

    [Serializable]
    public class LightMapInfo
    {
        public Renderer Renderer;
        public int LightmapIndex;
        public Vector4 LightmapScaleOffset;
    }

    [MenuItem("GameObject/Get Data")]
    [MenuItem("CONTEXT/PrefabLightMapData/Get Data")]
    public static void GetData()
    {
        var go = Selection.activeObject as GameObject;
        if (go == null)
        {
            Debug.Log("go == null");
            return;
        }

        var data = go.GetComponent<PrefabLightMapData>();
        if (data == null)
        {
            Debug.Log("data == null");
            return;
        }

        var renderers = go.GetComponentsInChildren<Renderer>();
        Debug.Log("renderers = " + renderers.Length);
        data.LightMapInfos = new LightMapInfo[renderers.Length];
        for (var i = 0; i < renderers.Length; i++)
        {
            var r = renderers[i];
            data.LightMapInfos[i] = new LightMapInfo()
            {
                Renderer = r,
                LightmapIndex = r.lightmapIndex,
                LightmapScaleOffset = r.lightmapScaleOffset
            };
        }
    }
}
