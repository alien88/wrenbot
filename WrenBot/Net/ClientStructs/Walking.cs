using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ClientStructs
{
    public class Walking
    {
        public byte Action { get { return 0x06; } set { } }
        public byte Ordinal { get; set; }
        public FaceDirection Direction { get; set; }
    }
}
