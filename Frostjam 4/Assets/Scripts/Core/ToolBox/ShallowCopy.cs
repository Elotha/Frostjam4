using System.Reflection;

namespace Core.ToolBox
{
    public static class ShallowCopy
    {
        /// <summary>
        /// Member-wise copy of a class
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetShallowCopy<T>(this T original) where T : new()
        {
            var clone = new T();

            // Get Fields of class
            var fields = original.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // Copy value of each field of class
            foreach (var fieldInfo in fields) fieldInfo.SetValue(clone, fieldInfo.GetValue(original));

            return clone;
        }

        /// <summary>
        /// Member-wise copy of a class
        /// </summary>
        /// <param name="original"></param>
        /// <param name="clone"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetShallowCopyNonAlloc<T>(this T original, ref T clone) where T : new()
        {
            // Get Fields of class
            var fields = original.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // Copy value of each field of class
            foreach (var fieldInfo in fields) fieldInfo.SetValue(clone, fieldInfo.GetValue(original));

            return clone;
        }
    }
}