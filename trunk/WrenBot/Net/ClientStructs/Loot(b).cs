using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class Lootb
    {
        public byte Action { get { return 0x07; } set { } }
        public byte Ordinal { get; set; }
        public byte Sequence { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public byte Unknown { get { return 0x00; } set { } }
    }
}
