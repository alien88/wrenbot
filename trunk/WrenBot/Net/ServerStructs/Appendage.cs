using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class Appendage
    {
        public byte Action { get { return 0x37; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public ushort Icon { get; set; }
        public byte NFI { get; set; }
        public string8 Name { get; set; }
    }
}
