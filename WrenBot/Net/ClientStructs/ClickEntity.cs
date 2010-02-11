using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    class ClickEntity
    {
        public byte Action { get { return 0x43; } set { } }
        public byte Ordinal { get; set; }
        public byte Orientation { get { return 0x01; } set { } }
        public uint Serial { get; set; }
    }
}
