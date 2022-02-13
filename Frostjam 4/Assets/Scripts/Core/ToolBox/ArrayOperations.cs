using System;
using Random = UnityEngine.Random;

namespace Core.ToolBox
{
    public static class ArrayOperations
    {
        public static T Find<T>(this T[] array, Predicate<T> predicate) => Array.Find(array, predicate);
        public static T[] FindAll<T>(this T[] array, Predicate<T> predicate) => Array.FindAll(array, predicate);
        public static bool Exists<T>(this T[] array, Predicate<T> predicate) => Array.Exists(array, predicate);
        public static int IndexOf<T>(this T[] array, Predicate<T> predicate) => Array.IndexOf(array, predicate);
        public static int IndexOf<T>(this T[] array, T value) => Array.IndexOf(array, value);
        public static T GetRandom <T>(this T[] array) => array[Random.Range(0, array.Length)];

    }
}
