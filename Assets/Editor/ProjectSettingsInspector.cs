#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


class ProjectSettingsInspector : EditorWindow
{
    //
    // Written by Peter Schraut
    //     http://www.console-dev.de
    //
    // The "Project Settings Inspector" window embeds all Unity project settings
    // in a single window.
    //
    // Store this file in your Unity project in a folder called "Editor".
    // You can open this "Project Settings Inspector" from Unity's main menu
    //     Mainmenu > Edit > Project Settings > Everything
    //
    // Download most recent version from:
    //     https://bitbucket.org/snippets/pschraut/5edXM8
    //
    Vector2 m_ScrollView;

    class Binding
    {
        public Editor editor;
        public string objectName;
        public float minHeight;

        public Binding(string objectName, float minSize)
        {
            this.objectName = objectName;
            this.minHeight = minSize;
        }
    }

    // These are the asset names of those project settings objects as found in memory.
    static Binding[] s_Bindings = new Binding[]
    {
        new Binding("InputManager", 0),
        new Binding("TagManager", 0),
        new Binding("AudioManager", 0),
        new Binding("TimeManager",  0),// Not in ProjectSettings folder
        new Binding("ProjectSettings", 0),
        new Binding("PlayerSettings", 0),
        new Binding("PhysicsManager",  800),// DynamicsManager.asset
        new Binding("Physics2DSettings", 1000),
        new Binding("QualitySettings", 0),
        new Binding("GraphicsSettings", 0),
        new Binding("NetworkManager", 0),
        new Binding("Editor Settings",  0),// EditorSettings.asset
        new Binding("MonoManager", 600), // Not in ProjectSettings folder (Script Execution Order)
    };


    void OnEnable()
    {
        titleContent = new GUIContent("ProjectSettings");

        var unityEngine = typeof(UnityEngine.Time).Assembly;
        var unityEditor = typeof(UnityEditor.EditorGUI).Assembly;
        var allObjects = Resources.FindObjectsOfTypeAll<UnityEngine.Object>();

        foreach (var binding in s_Bindings)
        {
            foreach (var obj in allObjects)
            {
                if (obj == null)
                    continue;

                // The name of the asset is usually the filename as well
                if (!string.Equals(obj.name, binding.objectName, System.StringComparison.OrdinalIgnoreCase))
                    continue;

                // Only consider Unity classes
                var assembly = obj.GetType().Assembly;
                if (assembly != unityEngine && assembly != unityEditor)
                    continue;

                // NetworkManager was found 3 times, so filter out the 2 of type 'MonoScript'
                // Uses this log to figure it out:
                // Debug.Log(obj.name + ", Type: " + obj.GetType().FullName + ", Assembly: " + obj.GetType().Assembly.FullName);
                if (obj.GetType() == typeof(MonoScript))
                    continue;

                binding.editor = Editor.CreateEditor(obj);
            }
        }
    }

    void OnDisable()
    {
        for (var n = 0; n < s_Bindings.Length; ++n)
        {
            if (s_Bindings[n] != null)
            {
                Editor.DestroyImmediate(s_Bindings[n].editor);
                s_Bindings[n].editor = null;
            }
        }
    }

    void OnGUI()
    {
        using (var scrollView = new EditorGUILayout.ScrollViewScope(m_ScrollView))
        {
            m_ScrollView = scrollView.scrollPosition;

            foreach (var binding in s_Bindings)
            {
                if (binding.editor == null)
                    continue;

                var editor = binding.editor;
                editor.DrawHeader();

                // We use the header rect as a clickable button for later.
                // However, the header has buttons itself on the right side,
                // so we adjust our rectangle to be smaller to not mess with the default buttons.
                var headerRect = GUILayoutUtility.GetLastRect();
                headerRect.width *= 0.75f;

                // Use a guid to avoid name clashes
                var editorPrefsKey = "daf29d3b-0aca-45c6-b529-df6c834be417/" + editor.target.name;
                var isExpanded = EditorPrefs.GetBool(editorPrefsKey, false);

                // Draw an invisble button over the header
                if (GUI.Button(headerRect, "", GUI.skin.label))
                {
                    EditorPrefs.SetBool(editorPrefsKey, !isExpanded);
                    continue;
                }

                if (!isExpanded)
                    continue;

                // Draw the editor
                using (new EditorGUILayout.VerticalScope(GUILayout.MinHeight(binding.minHeight)))
                {
                    editor.OnInspectorGUI();
                }
            }
        }
    }

    [MenuItem("Edit/Project Settings/Everything", priority = 0)]
    static void OpenWindowMenuItem()
    {
        var wnd = GetWindow<ProjectSettingsInspector>();
        if (wnd != null)
            wnd.Show();
    }
}
#endif
