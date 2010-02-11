using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class UseSpell2
    {
        public byte Action { get { return 0x0F; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
    }
}