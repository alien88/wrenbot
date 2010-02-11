using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    class Assail
    {
        public byte Action { get { return 0x13; } set { } }
        public byte Ordinal { get; set; }
        public byte Sequence { get { return 0x01; } set { } }
    }
}
