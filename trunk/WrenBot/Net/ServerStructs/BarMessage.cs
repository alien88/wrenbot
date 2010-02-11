using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class BarMessage
    {
        public byte Action { get { return 0x0A; } set { } }
        public byte Ordinal { get; set; }
        public byte Type { get; set; }
        public byte NFI { get; set; }
        public string8 Message { get; set; }
    }
}
