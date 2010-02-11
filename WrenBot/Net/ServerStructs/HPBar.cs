using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class HPBAR
    {
        public byte Action { get { return 0x13; } set { } }
        public byte Ordinal { get; set; }
        public uint Serial { get; set; }
        public byte NFI { get; set; }
        public byte Percent { get; set; }
        public byte Sound { get; set; }
    }
}
