using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    public class AddSingleSprite
    {
        public byte Action { get { return 0x07; } set { } }
        public byte Ordinal { get; set; }
        public byte Number { get { return 0x01; } set { } }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public uint nfi { get { return 0x000630D0; } set { } }
        public ushort spriteid { get; set; }
    }
}