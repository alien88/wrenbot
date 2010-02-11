using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class ItemSlotInfo
    {
        public byte Action { get { return 0x0F; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public ushort IconSet { get; set; }
        public byte Icon { get; set; }
        public string8 Name { get; set; }
        public uint Amount { get; set; }
        public bool Stackable { get; set; }
        public uint CurrentDurability { get; set; }
        public uint MaximumDurability { get; set; }
    }
}
