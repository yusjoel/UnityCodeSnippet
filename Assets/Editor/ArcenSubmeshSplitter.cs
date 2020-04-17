using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArcenSubmeshSplitter
{
    //http://answers.unity3d.com/questions/1213025/separating-submeshes-into-unique-meshes.html
    [MenuItem("Arcen/Submesh Splitter")]
    public static void BuildWindowsAssetBundle()
    {
        GameObject[] objects = Selection.gameObjects;
        for (int i = 0; i < objects.Length; i++)
        {
            ProcessGameObject(objects[i]);
        }
        Debug.Log("Done splitting meshes into submeshes!  " + System.DateTime.Now);
    }

    public class MeshFromSubmesh
    {
        public Mesh mesh;
        public int id; // Represent the ID of the sub mesh from with the new 'mesh' has been created
    }

    private static void ProcessGameObject(GameObject go)
    {
        // Isolate Sub Meshes
        MeshFilter meshFilterComponent = go.GetComponent<MeshFilter>();
        if (!meshFilterComponent)
        {
            Debug.LogError("MeshFilter null for '" + go.name + "'!");
            return;
        }
        MeshRenderer meshRendererComponent = go.GetComponent<MeshRenderer>();
        if (!meshRendererComponent)
        {
            Debug.LogError("MeshRenderer null for '" + go.name + "'!");
            return;
        }
        Mesh mesh = go.GetComponent<MeshFilter>().sharedMesh;
        if (!mesh)
        {
            Debug.LogError("Mesh null for '" + go.name + "'!");
            return;
        }
        List<MeshFromSubmesh> meshFromSubmeshes = GetAllSubMeshAsIsolatedMeshes(mesh);
        if (meshFromSubmeshes == null || meshFromSubmeshes.Count == 0)
        {
            Debug.LogError("List<MeshFromSubmesh> empty or null for '" + go.name + "'!");
            return;
        }
        string goName = go.name;
        for (int i = 0; i < meshFromSubmeshes.Count; i++)
        {
            string meshFromSubmeshName = goName + "_sub_" + i;
            GameObject meshFromSubmeshGameObject = new GameObject();
            meshFromSubmeshGameObject.name = meshFromSubmeshName;
            meshFromSubmeshGameObject.transform.SetParent(meshFilterComponent.transform);
            meshFromSubmeshGameObject.transform.localPosition = Vector3.zero;
            meshFromSubmeshGameObject.transform.localRotation = Quaternion.identity;
            MeshFilter meshFromSubmeshFilter = meshFromSubmeshGameObject.AddComponent<MeshFilter>();
            meshFromSubmeshFilter.sharedMesh = meshFromSubmeshes[i].mesh;
            MeshRenderer meshFromSubmeshMeshRendererComponent = meshFromSubmeshGameObject.AddComponent<MeshRenderer>();
            if (meshRendererComponent != null)
            {
                // To use the same mesh renderer properties of the initial mesh
                EditorUtility.CopySerialized(meshRendererComponent, meshFromSubmeshMeshRendererComponent);
                // We just need the only one material used by the sub mesh in its renderer
                Material material = meshFromSubmeshMeshRendererComponent.sharedMaterials[meshFromSubmeshes[i].id];
                meshFromSubmeshMeshRendererComponent.sharedMaterials = new[] { material };
            }
            // Don't forget to save the newly created mesh in the asset database (on disk)
            string path = "Assets/_Meshes/Split/" + meshFromSubmeshName + ".asset";
            AssetDatabase.CreateAsset(meshFromSubmeshes[i].mesh, path);
            Debug.Log("Created: " + path);
        }
    }

    private static List<MeshFromSubmesh> GetAllSubMeshAsIsolatedMeshes(Mesh mesh)
    {
        List<MeshFromSubmesh> meshesToReturn = new List<MeshFromSubmesh>();
        if (!mesh)
        {
            Debug.LogError("No mesh passed into GetAllSubMeshAsIsolatedMeshes!");
            return meshesToReturn;
        }
        int submeshCount = mesh.subMeshCount;
        if (submeshCount < 2)
        {
            Debug.LogError("Only " + submeshCount + " submeshes in mesh passed to GetAllSubMeshAsIsolatedMeshes");
            return meshesToReturn;
        }
        MeshFromSubmesh m1;
        for (int i = 0; i < submeshCount; i++)
        {
            m1 = new MeshFromSubmesh();
            m1.id = i;
            m1.mesh = mesh.GetSubmesh(i);
            meshesToReturn.Add(m1);
        }
        return meshesToReturn;
    }
}