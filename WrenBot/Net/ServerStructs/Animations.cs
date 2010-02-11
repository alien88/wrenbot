using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    public class Animation
    {
        public byte Action { get { return 0x29; } set { } }
        public byte Ordinal { get; set; }
        public uint ToWho { get; set; }
        public uint FromWho { get; set; }
        public ushort Number { get; set; }
        public ushort Speed { get; set; }
    }
}