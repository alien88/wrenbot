using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class PlaySound
    {
        public byte Action { get { return 0x19; } set { } }
        public byte Ordinal { get; set; }
        public byte Number { get; set; }
    }
}
