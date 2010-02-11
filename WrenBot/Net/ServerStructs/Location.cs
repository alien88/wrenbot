using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class PlayerLocation
    {
        public byte Action { get { return 0x04; } set { } }
        public byte Ordinal { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
    }
}
