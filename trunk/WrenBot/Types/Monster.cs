using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Types
{
    /// <summary>
    /// Monster Object (Inherits MapEntity)
    /// </summary>
    public class Monster : MapEntity
    {
        /// <summary>
        /// Default Monster Constructor
        /// </summary>
        public Monster() : base(Type.Monster) { this.Animations = new List<Animation>(); }

        /// <summary>
        /// Boolean: Is Monster Pet?
        /// </summary>
        public bool IsPet { get; set; }

        /// <summary>
        /// HP Percentage
        /// </summary>
        public byte HPPercent { get; set; }

        /// <summary>
        /// List Of Animations Dealt To Monster
        /// </summary>
        public List<Animation> Animations { get; set; }

        public bool Targeted { get; set; }

        public DateTime Date { get; set; }
    }
}
