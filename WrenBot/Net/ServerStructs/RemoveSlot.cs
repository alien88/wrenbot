using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class RemoveSlot
    {
        public enum SlotType
        {
            Item = 0x10,
            Spell = 0x18,
            Skill = 0x2D
        }
        public SlotType RemoveSlotType { get; set; }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
    }
}
