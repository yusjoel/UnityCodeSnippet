using UnityEngine;

namespace Extensions
{
    public static class LayerMaskExtension
    {
        public static void SetLayerMask(this GameObject go, string layerName)
        {
            if (!go) return;
            int layer = LayerMask.NameToLayer(layerName);
            if (layer < 0) return;

            SetLayerMaskRecursively(go.transform, layer);
        }

        private static void SetLayerMaskRecursively(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                var child = transform.GetChild(i);
                SetLayerMaskRecursively(child, layer);
            }
        }
    }
}
