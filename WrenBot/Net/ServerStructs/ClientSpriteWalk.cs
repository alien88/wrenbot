using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    class ClientSpriteWalk
    {
        public byte Action { get { return 0x0B; } set { } }
        public byte Ordinal { get; set; }
        public FaceDirection Direction { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort NFIA { get { return 0x000B; } set { } }
        public ushort NFIB { get { return 0x000B; } set { } }
        public byte NFIC { get { return 0x01; } set { } }
    }
}
