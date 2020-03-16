namespace Extensions
{
    public static class ArrayExtension
    {
        public static T SafelyGet<T>(this T[] array, int index)
        {
            if (array == null) return default(T);
            if (index < 0 || index >= array.Length) return default(T);
            return array[index];
        }

        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
    }
}
