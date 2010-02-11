using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class SpellSlotInfo
    {
        public byte Action { get { return 0x17; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public ushort Icon { get; set; }
        public byte TargetType { get; set; }
        public string8 Name { get; set; }
        public string8 Prompt { get; set; }
        public byte Lines { get; set; }
    }
}
