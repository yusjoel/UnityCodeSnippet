using System.Collections.Generic;

namespace Extensions
{
    public static class ListExtension
    {
        public static bool IsNullOrEmpty<T>(this IList<T> list)
        {
            if (list != null)
                return list.Count == 0;
            return true;
        }

        public static void AddUnique<T>(this IList<T> list, T item)
        {
            if (list == null)
                return;
            if (!list.Contains(item))
                list.Add(item);
        }

        /// <summary>
        /// 安全地获取表项, 如果索引超出范围, 那么返回null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T Get<T>(this IList<T> list, int index) where T : class
        {
            if (index < 0 || index >= list.Count)
                return null;
            return list[index];
        }
    }
}
