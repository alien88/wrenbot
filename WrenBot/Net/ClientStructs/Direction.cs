using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ClientStructs
{
    class Direction
    {
        public byte Action { get { return 0x11; } set { } }
        public byte Ordinal { get; set; }
        public FaceDirection FaceDirection { get; set; }
    }
}