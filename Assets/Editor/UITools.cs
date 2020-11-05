using Scripts;
using UnityEditor;
using UnityEngine;

public class UITools
{
    [MenuItem("CONTEXT/RectTransform/Add SafeAreaPanel")]
    private static void AddSafeAreaPanel()
    {
        var safeAreaPanel = AddChild("SafeAreaPanel");
        safeAreaPanel.gameObject.AddComponent<SafeAreaPanel>();
    }

    [MenuItem("CONTEXT/RectTransform/Add Child")]
    private static void AddChild()
    {
        AddChild("Child");
    }

    //[MenuItem("CONTEXT/RectTransform/Add Anchor/Top Left")]
    //private static void AddAnchorTopLeft()
    //{
    //    AddChild("TopLeftAnchor");
    //}

    //[MenuItem("CONTEXT/RectTransform/Add Anchor/Top Center")]
    //private static void AddAnchorTopCenter()
    //{
    //    AddChild("TopCenterAnchor");
    //}

    //[MenuItem("CONTEXT/RectTransform/Add Anchor/Top Right")]
    //private static void AddAnchorTopRight()
    //{
    //    AddChild("TopRightAnchor");
    //}

    private static RectTransform AddChild(string objectName)
    {
        var selectedObject = Selection.activeObject as GameObject;
        if (selectedObject == null)
            return null;

        var parent = selectedObject.GetComponent<RectTransform>();
        if (parent == null)
            return null;
        return AddChild(parent, objectName);
    }

    private static RectTransform AddChild(RectTransform parent, string objectName)
    {
        var child = new GameObject(objectName, typeof(RectTransform));
        child.transform.SetParent(parent);
        child.layer = parent.gameObject.layer;

        var rectTransform = child.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.localScale = Vector3.one;
        rectTransform.localEulerAngles = Vector3.zero;

        return rectTransform;
    }
}
