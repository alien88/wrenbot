using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class DropItem
    {
        public byte Action { get { return 0x08; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public byte NFI { get { return 0x00; } set { } }
        public ushort NFI2 { get { return 0x0000; } set { } }
        public byte Ammount { get; set; }
    }
}
