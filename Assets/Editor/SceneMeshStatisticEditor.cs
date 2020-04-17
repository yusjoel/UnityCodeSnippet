namespace Gempoll.Editor
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class SceneMeshStatisticEditor : EditorWindow
    {
        [MenuItem("Tools/场景中的面数")]
        private static void ShowSceneMeshStatistic()
        {
            bool isInludeinactive = true;
            Dictionary<string, SceneMeshStatisticResult> sceneMeshStatistics = new Dictionary<string, SceneMeshStatisticResult>();
            Action<bool> reset = (bool inludeinactive) =>
            {
                sceneMeshStatistics.Clear();
                if ( EditorSceneManager.sceneCount > 0 )
                {
                    for ( int i = 0; i < EditorSceneManager.sceneCount; ++i )
                    {
                        var scene = EditorSceneManager.GetSceneAt(i);
                        if ( scene.isLoaded )
                        {
                            var result = GetSceneMeshStatistic(scene, inludeinactive);
                            sceneMeshStatistics.Add(scene.path, result);
                        }
                    }
                }
            };
            reset(isInludeinactive);
            var wndEditor = EditorWindow.GetWindow<SceneMeshStatisticEditor>(false);
            wndEditor.titleContent = new GUIContent("场景中的面数");
            wndEditor.DoOnGUI = () =>
            {
                isInludeinactive = GUILayout.Toggle(isInludeinactive, "是否包含Inactive对象");
                if (GUILayout.Button("刷新", GUILayout.Width(50)))
                {
                    reset(isInludeinactive);
                }
                GUILayout.Space(20);
                foreach (var pair in sceneMeshStatistics )
                {
                    GUILayout.Label(string.IsNullOrEmpty(pair.Key) ? "Untitled" : pair.Key);
                    GUILayout.Label(string.Format("GameObject总数：{0}", pair.Value.TotalGameObjectCount));
                    GUILayout.Label(string.Format("mesh: {0}\ntriangle: {1}\nvertext: {2}", GetSize(pair.Value.MeshCount), GetSize(pair.Value.TriangleCount), GetSize(pair.Value.VertextCount)));
                    if ( pair.Value.MostMesh != null && pair.Value.MostMesh.Object != null )
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.ObjectField("面数最多的物体：", pair.Value.MostMesh.Object, typeof(GameObject), true);
                        EditorGUILayout.EndHorizontal();
                        GUILayout.Label(string.Format("mesh: {0}\ntriangle: {1}\nvertext: {2}", GetSize(pair.Value.MostMesh.MeshCount), GetSize(pair.Value.MostMesh.TriangleCount), GetSize(pair.Value.MostMesh.VertextCount)));
                    }
                    GUILayout.Space(20);
                }
            };
        }

        private static void ShowGameobjectMeshStatistic(GameObject gameobject, GameobjectMeshStatisticResult statistic=null)
        {
            if (gameobject != null && gameobject.transform.childCount > 0 )
            {
                bool isInludeinactive = true;
                SceneMeshStatisticResult sceneRes = null;
                Action<bool> reset = (bool inludeinactive) =>
                {
                    List<GameObject> gos = new List<GameObject>();
                    if ( gameobject.transform.childCount > 0 )
                    {
                        for ( int i = 0; i < gameobject.transform.childCount; ++i )
                        {
                            gos.Add(gameobject.transform.GetChild(i).gameObject);
                        }
                    }
                    sceneRes = MeshStatisticGameObjects(gos);
                    if (statistic == null)
                    {
                        statistic = MeshStatisticGameObjects(new GameObject[] { gameobject }).BiggestObject;
                    }
                    var goDelfStatistic = GetGameObjectSelfMeshSatistic(gameobject);
                    if (goDelfStatistic.IsBiggerThan(sceneRes.BiggestObject))
                    {
                        sceneRes.BiggestObject = goDelfStatistic;
                    }
                };
                reset(isInludeinactive);
                var wndEditor = EditorWindow.GetWindow<SceneMeshStatisticEditor>(false);
                wndEditor.titleContent = new GUIContent(string.Format("物体的面数: {0}", gameobject.name));
                wndEditor.DoOnGUI = () =>
                {
                    isInludeinactive = GUILayout.Toggle(isInludeinactive, "是否包含Inactive对象");
                    if ( GUILayout.Button("刷新", GUILayout.Width(50)) )
                    {
                        reset(isInludeinactive);
                    }
                    GUILayout.Space(20);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(string.Format("{0}:", string.IsNullOrEmpty(gameobject.name) ? "GameObject" : gameobject.name), gameobject, typeof(GameObject), true);
                    EditorGUILayout.EndHorizontal();
                    if (statistic != null)
                    {
                        GUILayout.Label(string.Format("mesh: {0}\ntriangle: {1}\nvertext: {2}", GetSize(statistic.MeshCount), GetSize(statistic.TriangleCount), GetSize(statistic.VertextCount)));
                    }
                    EditorGUILayout.BeginHorizontal();
                    if ( sceneRes.BiggestObject != null && sceneRes.BiggestObject.Object != null )
                    {
                        EditorGUILayout.ObjectField("面数最多的物体：", sceneRes.BiggestObject.Object, typeof(GameObject), true);
                        if ( GUILayout.Button("统计此物体") )
                        {
                            ShowGameobjectMeshStatistic(sceneRes.BiggestObject.Object, sceneRes.BiggestObject);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Label(string.Format("mesh: {0}\ntriangle: {1}\nvertext: {2}", GetSize(sceneRes.BiggestObject.MeshCount), GetSize(sceneRes.BiggestObject.TriangleCount), GetSize(sceneRes.BiggestObject.VertextCount)));
                    GUILayout.Space(20);
                };
            }
        }

        private Action DoOnGUI = null;

        private void OnDisable()
        {
            this.DoOnGUI = null;
        }

        private void OnDestroy()
        {
            this.DoOnGUI = null;
        }

        private void OnGUI()
        {
            this.DoOnGUI?.Invoke();
        }

        private static string GetSize(int count)
        {
            if ( count < 1000 )
                return count.ToString();

            if ( count < 1000 * 1024 )
            {
                float f = ( float )count / 1024;
                return f.ToString("0.0k");
            }

            if ( count < 1000 * 1024 * 1024 )
            {
                float f = ( float )count / 1024 / 1024;
                return f.ToString("0.0m");
            }

            return "Too large";
            //throw new ArgumentOutOfRangeException("count");
        }

        public class MeshStatisticResult
        {
            public int MeshCount = 0;
            public int TriangleCount = 0;
            public int VertextCount = 0;
        }
        public class GameobjectMeshStatisticResult : MeshStatisticResult
        {
            public GameObject Object = null;

            public bool IsBiggerThan(GameobjectMeshStatisticResult other)
            {
                if (other == null)
                {
                    return true;
                }
                return ( this.TriangleCount > 0 && this.TriangleCount > other.TriangleCount );
            }
        }
        public class SceneMeshStatisticResult : MeshStatisticResult
        {
            public GameobjectMeshStatisticResult BiggestObject = null;
            public GameobjectMeshStatisticResult MostMesh = null;
            public int TotalGameObjectCount = 0;
        }
        public static SceneMeshStatisticResult MeshStatisticGameObjects(ICollection<GameObject> gameobjects, bool includeInactive=false)
        {
            int meshCount = 0;
            int triangleCount = 0;
            int vertCount = 0;
            GameobjectMeshStatisticResult goMax = null;
            GameobjectMeshStatisticResult meshMax = null;
            int transformCount = 0;
            if (gameobjects != null && gameobjects.Count > 0)
            {
                foreach (var go in gameobjects)
                {
                    {
                        var transforms = go.GetComponentsInChildren<Transform>(includeInactive);
                        if (transforms != null)
                        {
                            transformCount += transforms.Length;
                        }
                    }
                    GameobjectMeshStatisticResult goRest = new GameobjectMeshStatisticResult();
                    goRest.Object = go;
                    {
                        var meshFilters = go.GetComponentsInChildren<MeshFilter>(includeInactive);
                        if ( meshFilters != null && meshFilters.Length > 0 )
                        {
                            foreach ( var filter in meshFilters )
                            {
                                if ( filter.sharedMesh != null )
                                {
                                    GameobjectMeshStatisticResult filterObj = new GameobjectMeshStatisticResult()
                                    {
                                        VertextCount = filter.sharedMesh.vertexCount,
                                        MeshCount = 1,
                                        TriangleCount = filter.sharedMesh.triangles.Length / 3,
                                        Object = filter.gameObject
                                    };
                                    goRest.MeshCount++;
                                    goRest.VertextCount += filterObj.VertextCount;
                                    goRest.TriangleCount += filterObj.TriangleCount;
                                    if (filterObj.IsBiggerThan(meshMax))
                                    {
                                        meshMax = filterObj;
                                    }
                                }
                            }
                        }
                    }
                    meshCount += goRest.MeshCount;
                    triangleCount += goRest.TriangleCount;
                    vertCount += goRest.VertextCount;
                    if (goRest.IsBiggerThan(goMax))
                    {
                        goMax = goRest;
                    }
                }
            }
            return new SceneMeshStatisticResult()
            {
                MeshCount = meshCount,
                TriangleCount = triangleCount,
                VertextCount = vertCount,
                BiggestObject = goMax,
                MostMesh = meshMax,
                TotalGameObjectCount = transformCount
            };
        }

        private static GameobjectMeshStatisticResult GetGameObjectSelfMeshSatistic(GameObject go)
        {
            GameobjectMeshStatisticResult goRest = new GameobjectMeshStatisticResult();
            goRest.Object = go;
            {
                var filter = go.GetComponent<MeshFilter>();
                if ( filter != null && filter.sharedMesh != null )
                {
                    goRest.MeshCount++;
                    goRest.VertextCount += filter.sharedMesh.vertexCount;
                    goRest.TriangleCount += filter.sharedMesh.triangles.Length / 3;
                }
            }
            return goRest;
        }

        public static SceneMeshStatisticResult GetSceneMeshStatistic(Scene scene, bool includeInactive = true)
        {
            if (scene != null)
            {
                var gos = scene.GetRootGameObjects();
                return MeshStatisticGameObjects(gos, includeInactive);
            }
            return new SceneMeshStatisticResult()
            {
                MeshCount = 0,
                TriangleCount = 0,
                VertextCount = 0,
                BiggestObject = null
            };
        }
    }
}
