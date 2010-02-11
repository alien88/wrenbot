using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class Serial
    {
        public byte Action { get { return 0x05; } set { } }
        public byte Ordinal { get; set; }
        public uint ID { get; set; }
    }
}
