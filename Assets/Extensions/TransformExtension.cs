using UnityEngine;

namespace Extensions
{
    public static class TransformExtension
    {
        public static void SetPositionX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
        }

        public static void SetPositionY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }

        public static void SetPositionZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
        }

        public static void SetLocalPositionX(this Transform t, float newX)
        {
            t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform t, float newY)
        {
            t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
        }

        public static void SetLocalPositionZ(this Transform t, float newZ)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZ);
        }

        public static void SetEulerAnglesX(this Transform t, float newX)
        {
            t.eulerAngles = new Vector3(newX, t.eulerAngles.y, t.eulerAngles.z);
        }

        public static void SetEulerAnglesY(this Transform t, float newY)
        {
            t.eulerAngles = new Vector3(t.eulerAngles.x, newY, t.eulerAngles.z);
        }

        public static void SetEulerAnglesZ(this Transform t, float newZ)
        {
            t.eulerAngles = new Vector3(t.eulerAngles.x, t.eulerAngles.y, newZ);
        }
    }
}
