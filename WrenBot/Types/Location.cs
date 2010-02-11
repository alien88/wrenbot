using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot
{
    public enum FaceDirection
    {
        Up = 0x00,
        Right = 0x01,
        Down = 0x02,
        Left = 0x03,
        None = 0x04
    }
}

namespace WrenBot.Types
{
    /// <summary>
    /// Location Object
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Default Location Constructor
        /// </summary>
        public Location()
        {
            this.Map = 0;
            this.X = 0;
            this.Y = 0;
            this.Direction = FaceDirection.None;
        }

        /// <summary>
        /// Location Clone
        /// </summary>
        /// <param name="Loc">Location To Clone</param>
        public Location(Location Loc)
        {
            this.Map = Loc.Map;
            this.X = Loc.X;
            this.Y = Loc.Y;
            this.Direction = Loc.Direction;
        }

        /// <summary>
        /// Map Location
        /// </summary>
        public ushort Map;
        /// <summary>
        /// X Location
        /// </summary>
        public ushort X;
        /// <summary>
        /// Y Location
        /// </summary>
        public ushort Y;

        /// <summary>
        /// Absolute X Location
        /// </summary>
        public ushort AbsX;
        /// <summary>
        /// Absolute Y Location
        /// </summary>
        public ushort AbsY;
        /// <summary>
        /// Absolute Facing Direction
        /// </summary>
        public FaceDirection AbsLocation;

        /// <summary>
        /// Last X Location
        /// </summary>
        public ushort LastX;

        /// <summary>
        /// Last Y Location
        /// </summary>
        public ushort LastY;

        /// <summary>
        /// Number Of Steps Walked
        /// </summary>
        public int Steps;

        /// <summary>
        /// Locations Facing Direction
        /// </summary>
        public FaceDirection Direction { get; set; }

        /// <summary>
        /// Distance From Object
        /// </summary>
        /// <param name="Location">Location To Find Distance From Current Location</param>
        /// <returns>Distance From Current Location</returns>
        public double DistanceFrom(Location Location)
        {
            double XDiff = (double)Math.Abs(Location.X - X);
            double YDiff = (double)Math.Abs(Location.Y - Y);
            if (XDiff > YDiff)
                return XDiff;
            else return YDiff;
            //return Math.Sqrt((Math.Abs(X - Location.X) * Math.Abs(X - Location.X)) + (Math.Abs(Y - Location.Y) * Math.Abs(Y - Location.Y)));
        }

        /// <summary>
        /// Boolean: Within Range Of Current Location?
        /// </summary>
        /// <param name="Location">Location To Check Distance From Current Location</param>
        /// <param name="MaxDistance">Maximum Location Distance</param>
        /// <returns></returns>
        public bool WithinRange(Location Location, double MaxDistance)
        {
            return DistanceFrom(Location) <= MaxDistance;
        }

        /// <summary>
        /// Boolean: Location Is In View Of Screen
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public bool OnScreenOf(Location Location)
        {
            return !(Math.Abs(Location.X - X) >= 14 || Math.Abs(Location.Y - Location.Y) >= 14);
        }

        /// <summary>
        /// Location Infront Of Current Location
        /// </summary>
        public Location InfrontOf
        {
            get
            {
                switch (Direction)
                {
                    case FaceDirection.Up:
                        {
                            return new Location()
                            {
                                X = X,
                                Y = (ushort)(Y - 1),
                                Map = Map
                            };
                        }
                    case FaceDirection.Down:
                        {
                            return new Location()
                            {
                                X = X,
                                Y = (ushort)(Y + 1),
                                Map = Map
                            };
                        }
                    case FaceDirection.Left:
                        {
                            return new Location()
                            {
                                X = (ushort)(X - 1),
                                Y = Y,
                                Map = Map
                            };
                        }
                    case FaceDirection.Right:
                        {
                            return new Location()
                            {
                                X = (ushort)(X + 1),
                                Y = Y,
                                Map = Map
                            };
                        }
                }
                return this;
            }
        }

        /// <summary>
        /// Implicit Conversion From Location To Point
        /// </summary>
        /// <param name="Location">Location To Convert</param>
        /// <returns>Point Representation Of Location</returns>
        public static implicit operator Point(Location Location)
        {
            return new Point() { X = Location.X, Y = Location.Y };
        }
    }
}