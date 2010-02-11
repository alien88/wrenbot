using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Location Point
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Boolean: Ignore Map Blocks?
        /// </summary>
        public bool IgnoreBlocks { get; set; }

        /// <summary>
        /// Point X
        /// </summary>
        public ushort X { get; set; }

        /// <summary>
        /// Point Y
        /// </summary>
        public ushort Y { get; set; }

        /// <summary>
        /// Implicit Conversion From Point To Location
        /// </summary>
        /// <param name="Point">Point To Convert</param>
        /// <returns>Location Converted From Point</returns>
        public static implicit operator Location(Point Point)
        {
            return new Location() { X = Point.X, Y = Point.Y };
        }
    }

    /// <summary>
    /// Save Points Object
    /// </summary>
    public class SavePoints
    {
        /// <summary>
        /// WayPoints List
        /// </summary>
        public List<Point> WayPoints { get; set; }

        /// <summary>
        /// BlockPoints List
        /// </summary>
        public List<Point> BlockPoints { get; set; }
    }
}
