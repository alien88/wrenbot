using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    class EntityDirection
    {
        public byte Action { get { return 0x11; } set { } }
        public byte Ordinal { get; set; }
        public uint Serial { get; set; }
        public FaceDirection FaceDirection { get; set; }
    }
}
