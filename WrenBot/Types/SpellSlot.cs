using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Spell Slot Object
    /// </summary>
    public class SpellSlot
    {
        /// <summary>
        /// Spell Slot Target Type
        /// </summary>
        public byte TargetType { get; set; }

        /// <summary>
        /// Spell Slot Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Spell Slot Prompt
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Spell Slot Lines
        /// </summary>
        public byte Lines { get; set; }
    }
}
