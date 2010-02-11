using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class UseSkill
    {
        public byte Action { get { return 0x3E; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
    }
}
