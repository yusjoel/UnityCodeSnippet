using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
// ReSharper disable UnusedMember.Local

public class ModelStatisticsEditor : EditorWindow
{
    private GameObject activeGameObject;

    private Node rootNode;

    private Vector2 scrollPosition;

    [MenuItem("Tools/模型统计工具")]
    private static void Create()
    {
        var me = GetWindow<ModelStatisticsEditor>();
        me.Load();
    }

    private void Load()
    {
        activeGameObject = Selection.activeGameObject;

        if (activeGameObject)
        {
            rootNode = new Node(activeGameObject.transform);
            rootNode.MakeTree();
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Load")) Load();

        if (rootNode == null || rootNode.Transform == null)
            GUILayout.Label("先选中一个物件, 然后按Load");
        else
            rootNode.OnGUI();
    }

    /// <summary>
    ///     模型内的节点
    /// </summary>
    private class Node
    {
        private static readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        ///     子节点
        /// </summary>
        public readonly List<Node> Children;

        /// <summary>
        ///     折叠
        /// </summary>
        public bool Collapse;

        /// <summary>
        ///     每多一个父节点, 多一级缩进
        /// </summary>
        public int Indent;

        /// <summary>
        ///     MeshFilter, 可以为空
        /// </summary>
        public readonly MeshFilter MeshFilter;

        /// <summary>
        ///     总渲染器数
        /// </summary>
        public int TotalMeshes;

        /// <summary>
        ///     总面数
        /// </summary>
        public int TotalTriangles;

        /// <summary>
        ///     总顶点数
        /// </summary>
        public int TotalVertexCount;

        /// <summary>
        ///     对应的Transform
        /// </summary>
        public readonly Transform Transform;

        public Node(Transform transform)
        {
            Transform = transform;
            Children = new List<Node>();
            MeshFilter = transform.GetComponent<MeshFilter>();
            Collapse = true;
        }

        /// <summary>
        ///     遍历生成树
        /// </summary>
        public void MakeTree()
        {
            if (!Transform)
                throw new NullReferenceException("Transform");

            int count = Transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = Transform.GetChild(i);
                var childNode = new Node(child);
                childNode.Indent = Indent + 1;
                Children.Add(childNode);
                childNode.MakeTree();
            }

            TotalVertexCount = 0;
            TotalTriangles = 0;

            if (MeshFilter)
            {
                if (MeshFilter.sharedMesh == null)
                {
                    Debug.LogWarningFormat(MeshFilter, "MeshFilter.sharedMesh is null");
                }
                else
                {
                    TotalVertexCount = MeshFilter.sharedMesh.vertexCount;
                    TotalTriangles = MeshFilter.sharedMesh.triangles.Length / 3;
                    TotalMeshes = 1;
                }
            }

            foreach (var child in Children)
            {
                TotalVertexCount += child.TotalVertexCount;
                TotalTriangles += child.TotalTriangles;
                TotalMeshes += child.TotalMeshes;
            }
        }

        public void OnGUI()
        {
            string message = string.Format("{0}{1}Tris: {2} Verts:{3} Meshes:{4}",
                MakeIndent(), Collapse ? "+" : "-", GetSize(TotalTriangles), GetSize(TotalVertexCount),
                GetSize(TotalMeshes));
            GUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField("", Transform, typeof(Transform), true);
            EditorGUILayout.LabelField(message);
            if (Collapse)
            {
                if (GUILayout.Button("+")) Collapse = false;
            }
            else
            {
                if (GUILayout.Button("-")) Collapse = true;
            }

            GUILayout.EndHorizontal();

            if (!Collapse)
                foreach (var child in Children)
                    child.OnGUI();
        }

        /// <summary>
        ///     获取xxx.xk, xxx.xm之类的计数方法
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private string GetSize(int count)
        {
            if (count < 1000)
                return count.ToString();

            if (count < 1000 * 1024)
            {
                float f = (float) count / 1024;
                return f.ToString("0.0k");
            }

            if (count < 1000 * 1024 * 1024)
            {
                float f = (float) count / 1024 / 1024;
                return f.ToString("0.0m");
            }

            return "Too large";

            //throw new ArgumentOutOfRangeException("count");
        }

        public string MakeIndent()
        {
            sb.Length = 0;
            for (int i = 0; i < Indent; i++) sb.Append("----");

            return sb.ToString();
        }
    }
}
