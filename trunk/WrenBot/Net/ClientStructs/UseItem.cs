using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class UseItem
    {
        public byte Action { get { return 0x1C; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
    }
}
