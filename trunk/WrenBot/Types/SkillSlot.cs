using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Skill Slot Object
    /// </summary>
    public class SkillSlot
    {
        /// <summary>
        /// Skill Slot
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Skill Icon
        /// </summary>
        public ushort Icon { get; set; }

        /// <summary>
        /// Skill Name
        /// </summary>
        public string Name { get; set; }
    }
}
