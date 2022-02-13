using System.Collections.Generic;

namespace Core.ToolBox
{
    public static class PoolUtils
    {
        /// <summary>
        /// Removes object from list but does not preserve order
        /// </summary>
        /// <param name="list"></param>
        /// <param name="toRemove"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveEfficient<T>(this List<T> list, T toRemove)
        {
            var listCount = list.Count;
            if (listCount > 0)
            {
                var removeIndex = list.IndexOf(toRemove);
                var lastIndex = listCount - 1;
                list[removeIndex] = list[lastIndex];
                list.RemoveAt(lastIndex);
            }
        }
    }
}