using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class SpriteAnimation
    {
        public byte Action { get { return 0x1A; } set { } }
        public byte Ordinal { get; set; }
        public uint ID { get; set; }
        public byte Animation { get; set; }
        public byte Unknown1 { get; set; }
        public byte Speed { get; set; }
    }
}
