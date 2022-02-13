using System;

namespace Core.ToolBox
{
    /// <summary>
    /// Range is used to hold the min and max positions of our total tilemap
    /// </summary>
    [Serializable]
    public struct Range
    {
        public float max;
        public float min;
        public float size;

        /// <summary>
        /// Creates range with given min and max values
        /// </summary>
        /// <param name="max">Maximum point of range</param>
        /// <param name="min">Minimum point of range</param>
        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
            size = max - min;
        }

        /// <summary>
        /// Shifts the current range by the given amount
        /// </summary>
        /// <param name="offset">Amount to shift by</param>
        public void Shift(float offset)
        {
            max += offset;
            min += offset;
        }
    }
}