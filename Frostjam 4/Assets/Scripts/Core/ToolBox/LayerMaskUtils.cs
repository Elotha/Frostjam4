using UnityEngine;

namespace Core.ToolBox
{
    public static class LayerMaskUtils
    {
        public static bool Contains(this LayerMask layerMask, int layer) => (layerMask & 1 << layer) != 0;
    }
}