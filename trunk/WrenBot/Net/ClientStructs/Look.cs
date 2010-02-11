using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    class Look
    {
        public byte Action { get { return 0x0A; } set { } }
        public byte Ordinal { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
    }
}
