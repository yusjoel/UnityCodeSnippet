using UnityEngine;

namespace Extensions
{
    public static class PlayerPrefsExtension
    {
        public static bool GetBoolean(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public static void SetBoolean(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}
