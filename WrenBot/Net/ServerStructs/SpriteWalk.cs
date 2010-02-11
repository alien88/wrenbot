using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;
namespace WrenBot.Net.ServerStructs
{
    public class SpriteWalk
    {
        public byte Action { get { return 0x0C; } set { } }
        public byte Ordinal { get; set; }
        public uint Serial { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public FaceDirection Direction { get; set; }
    }
    
}
