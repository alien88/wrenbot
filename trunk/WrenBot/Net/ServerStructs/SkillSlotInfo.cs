using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class SkillSlotInfo
    {
        public byte Action { get { return 0x2C; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public ushort Icon { get; set; }
        public string8 Name { get; set; }
    }
}
