using UnityEditor;

namespace Assets.Editor
{
    public class SerializedVersionTool
    {
        [MenuItem("Tools/Upgrade Serialized Version")]
        private static void UpgradeSerializedVersion()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}
