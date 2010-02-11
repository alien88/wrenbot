using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Item Slot Object
    /// </summary>
    public class ItemSlot
    {
        /// <summary>
        /// Item Icon Set
        /// </summary>
        public ushort IconSet { get; set; }
        /// <summary>
        /// Item Icon
        /// </summary>
        public byte Icon { get; set; }
        /// <summary>
        /// Item Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Item Amount
        /// </summary>
        public uint Amount { get; set; }

        /// <summary>
        /// Items Current Durability
        /// </summary>
        public uint CurrentDurability { get; set; }

        /// <summary>
        /// Items Maximum Durability
        /// </summary>
        public uint MaximumDurability { get; set; }

        public bool IsDropped { get; set; }
    }
}
