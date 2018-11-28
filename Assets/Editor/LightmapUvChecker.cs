using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// (srcData.GetChannelMask() & copyChannels) == copyChannels
// issue: https://issuetracker.unity3d.com/issues/assertion-srcinfo-dot-getchannelmask-and-copychannels-equals-equals-copychannels-after-marking-mesh-without-lightmaps-uvs-as-lightmap-static
// question: https://answers.unity.com/questions/1470570/getting-some-error-always-when-i-open-unity.html
// RaymondEllis:
// > Apparently this is an issue with meshes without UV being set as lightmap static.
// > Simple fix: for every static object in your scene, uncheck "Lightmap Static".
// ThePilgrim:
// > I had this same error. I had a static mesh without UVs in my scene. To fix it, I went to the import settings on that mesh and checked "Generate Lightmap UVs".

public class LightmapUvChecker : EditorWindow
{
    private readonly Dictionary<string, List<MeshFilter>> modelPathMeshesMap = new Dictionary<string, List<MeshFilter>>();
    private int countOfMeshFilter;
    private int countOfModel;
    private Vector2 scrollPosition;
    private bool willRefresh;

    [MenuItem("Tools/Lightmap UV Checker")]
    private static void Create()
    {
        var me = GetWindow<LightmapUvChecker>();
        me.Load();
    }

    private void AddMeshFilter(MeshFilter meshFilter)
    {
        var mesh = meshFilter.sharedMesh;

        string assetPath = AssetDatabase.GetAssetPath(mesh);
        if(string.IsNullOrEmpty(assetPath))
            assetPath = "(N/A)";

        if (!modelPathMeshesMap.ContainsKey(assetPath))
        {
            modelPathMeshesMap[assetPath] = new List<MeshFilter>();
            if (assetPath != "(N/A)")
                countOfModel++;
        }

        modelPathMeshesMap[assetPath].Add(meshFilter);
        countOfMeshFilter++;
    }

    private void Load()
    {
        modelPathMeshesMap.Clear();
        countOfMeshFilter = 0;
        countOfModel = 0;

        var meshFilters = FindObjectsOfType<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            var mesh = meshFilter.sharedMesh;
            if (mesh == null)
                continue;

            var flags = GameObjectUtility.GetStaticEditorFlags(meshFilter.gameObject);
            bool isLightmapStatic = (flags & StaticEditorFlags.LightmapStatic) != 0;
            if (isLightmapStatic && IsNullOrEmpty(mesh.uv) && IsNullOrEmpty(mesh.uv2))
                AddMeshFilter(meshFilter);
        }
    }

    private bool IsNullOrEmpty<T>(T[] array)
    {
        if (array == null) return true;
        if (array.Length == 0) return true;
        return false;
    }

    private void OnGUI()
    {
        if (willRefresh)
        {
            willRefresh = false;
            Load();
        }

        DrawTitle();

        GUILayout.Space(10);

        DrawContent();
    }

    private void DrawContent()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        foreach (var pair in modelPathMeshesMap)
        {
            GUILayout.BeginHorizontal();

            string assetPath = pair.Key;
            var model = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (model)
                EditorGUILayout.ObjectField(model.name, model, typeof(GameObject), true);
            else
                GUILayout.Label("(N/A)");

            var meshFilters = pair.Value;
            if (GUILayout.Button("Remove Lightmap Static")) RemoveLightmapStatic(meshFilters);

            GUI.enabled = model != null;
            if (GUILayout.Button("Generate Lightmap UVs")) GenerateLightmapUvs(assetPath);
            GUI.enabled = true;

            GUILayout.EndHorizontal();

            foreach (var meshFilter in meshFilters)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.ObjectField(meshFilter.name, meshFilter, typeof(MeshFilter), true);
                var mesh = meshFilter.sharedMesh;
                EditorGUILayout.ObjectField(mesh.name, mesh, typeof(Mesh), true);
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndScrollView();
    }

    private void DrawTitle()
    {
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Refresh"))
            Load();

        if (GUILayout.Button("Remove Lightmap Static"))
            foreach (var pair in modelPathMeshesMap)
            {
                var meshFilters = pair.Value;
                RemoveLightmapStatic(meshFilters);
            }

        if (GUILayout.Button("Generate Lightmap UVs"))
            foreach (var pair in modelPathMeshesMap)
            {
                string assetPath = pair.Key;
                GenerateLightmapUvs(assetPath);
            }

        GUILayout.Label(string.Format("Model: {0} / Mesh: {1}", countOfModel, countOfMeshFilter));

        GUILayout.EndHorizontal();
    }

    private void GenerateLightmapUvs(string assetPath)
    {
        var modelImporter = AssetImporter.GetAtPath(assetPath) as ModelImporter;
        if (modelImporter == null)
            return;
        modelImporter.generateSecondaryUV = true;
        modelImporter.SaveAndReimport();
        willRefresh = true;
    }

    private void RemoveLightmapStatic(List<MeshFilter> meshFilters)
    {
        foreach (var meshFilter in meshFilters)
        {
            var go = meshFilter.gameObject;
            var flags = GameObjectUtility.GetStaticEditorFlags(go);
            var newFlags = flags & ~StaticEditorFlags.LightmapStatic;
            //Debug.LogFormat(go, "Old Flags:{0}\nNew Flags: {1}", flags, newFlags);
            GameObjectUtility.SetStaticEditorFlags(go, newFlags);
            EditorSceneManager.MarkSceneDirty(go.scene);
            willRefresh = true;
        }
    }
}
