using UnityEngine;

namespace Extensions
{
    public static class CameraExtension
    {
        public static bool See(this Camera camera, Vector3 worldPosition)
        {
            if (camera == null)
                return false;
            var viewportPoint = camera.WorldToViewportPoint(worldPosition);
            return viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1;
        }
    }
}
