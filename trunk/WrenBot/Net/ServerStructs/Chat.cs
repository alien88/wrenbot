using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class Chat
    {
        public byte Action { get { return 0x0D; } set { } }
        public byte Ordinal { get; set; }
        public byte Type { get; set; }
        public uint Serial { get; set; }
        public string8 Message { get; set; }
    }
}
