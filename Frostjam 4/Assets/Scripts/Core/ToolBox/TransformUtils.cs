using UnityEngine;

namespace Core.ToolBox
{
    public static class TransformUtils
    {
        public static bool HasParent(this Transform transform) => transform.parent != null;
        public static void AssertHasParent(this Transform transform)
        { 
            if(!transform.HasParent()) Debug.LogError(transform.gameObject.name + " does not have parent!");
        }
    }
}