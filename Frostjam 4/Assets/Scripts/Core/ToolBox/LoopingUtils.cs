using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.ToolBox
{
    public static class LoopingUtils
    {
        /// <summary>
        /// Scales a given BoundsInt in the X axis by given scale
        /// </summary>
        /// <param name="boundsInt">BoundsInt to be scaled in the X axis</param>
        /// <param name="scale">value to scale the given bounds by</param>
        /// <returns></returns>
        public static BoundsInt ScaleX(this BoundsInt boundsInt, float scale)
        {
            boundsInt.position = boundsInt.position.ScaleX(scale);
            boundsInt.size = boundsInt.size.ScaleX(scale);
            return boundsInt;
        }

        /// <summary>
        /// Scales the x value of the Vector3Int and floors it to int
        /// </summary>
        /// <param name="vectorInt">Vector to be scaled</param>
        /// <param name="scale">Value the vector will be scaled by</param>
        /// <returns>Vector with X value scaled by given value</returns>
        public static Vector3Int ScaleX(this Vector3Int vectorInt, float scale)
        {
            vectorInt.x = Mathf.FloorToInt(vectorInt.x * scale);
            return vectorInt;
        }

        /// <summary>
        /// Finds all children of a transform at a given depth
        /// </summary>
        /// <param name="transform">Parent Transform to find the child of</param>
        /// <param name="level">Depth level of children to be found</param>
        /// <returns>IEnumerable of all children</returns>
        public static IEnumerable<Transform> GetChildrenAtLevel(this Transform transform, int level)
        {
            if (level == 0) return new[] {transform};

            return Enumerable.Range(0, transform.childCount).SelectMany(index => transform.GetChild(index).GetChildrenAtLevel(level - 1));
        }


        /// <summary>
        /// Moves a Transform by given distance in the X axis
        /// </summary>
        /// <param name="transform">Transform to be moved</param>
        /// <param name="distance">Distance to move transform</param>
        public static void MoveX(this Transform transform, float distance)
        {
            var position = transform.position;
            position.x += distance;
            transform.position = position;
        }

        /// <summary>
        /// Moves all Transforms by given distance in the X axis
        /// </summary>
        /// <param name="transforms">Enumerable of Transforms to be moved</param>
        /// <param name="distance">Distance to move transforms</param>
        public static void MoveAllX(this IEnumerable<Transform> transforms, float distance)
        {
            foreach (var transform in transforms) transform.MoveX(distance);
        }
    }
}