using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class WindowResponse
    {
        public byte Action { get { return 0x0A; } }
        public byte Ordinal { get; set; }
        public byte Type { get { return 0x0A; } set { } }
        public string16 Message { get; set; }
    }
}
