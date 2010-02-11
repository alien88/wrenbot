using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class RemoveSprite
    {
        public byte Action { get { return 0x0E; } set { } }
        public byte Ordinal { get; set; }
        public uint ID { get; set; }
    }
}
