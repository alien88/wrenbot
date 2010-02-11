using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Animation Event Delegate
    /// </summary>
    /// <param name="Animation">Animation</param>
    public delegate void AnimationHandler(Animation Animation);

    /// <summary>
    /// Animation Object
    /// </summary>
    public class Animation
    {
        /// <summary>
        /// Default Animation Constructor
        /// </summary>
        /// <param name="ToWho">Serial Number To</param>
        /// <param name="FromWho">Serial Number From</param>
        /// <param name="Number">Animation Number</param>
        /// <param name="Speed">Animation Speed</param>
        public Animation(uint ToWho, uint FromWho, ushort Number, uint Speed)
        {
            this.ToWho = ToWho;
            this.FromWho = FromWho;
            this.Number = Number;
            this.Speed = Speed;
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Time Of Animation
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// Time Elapsed Since Animation
        /// </summary>
        public TimeSpan TimeElapsed
        {
            get { return DateTime.Now - Time; }
        }

        /// <summary>
        /// Entity Serial To
        /// </summary>
        public uint ToWho;

        /// <summary>
        /// Entity Serial From
        /// </summary>
        public uint FromWho;

        /// <summary>
        /// Animation Number
        /// </summary>
        public ushort Number;

        /// <summary>
        /// Animation Speed
        /// </summary>
        public uint Speed;

        /// <summary>
        /// Animation Event Handler
        /// </summary>
        public AnimationHandler OnAnimation;
    }
}
