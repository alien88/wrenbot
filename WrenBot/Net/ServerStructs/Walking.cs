using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;
using WrenBot;

namespace WrenBot.Net.ServerStructs
{
    public class SelfWalking
    {
        public byte Action { get { return 0x32; } set { } }
        public byte Ordinal { get; set; }
        public FaceDirection Direction { get; set; }
    }
}
