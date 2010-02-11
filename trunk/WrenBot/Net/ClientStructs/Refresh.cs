using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class Refresh
    {
        public byte Action { get { return 0x38; } set { } }
        public byte Ordinal { get; set; }
        public byte Sequence { get { return 0x00; } set { } }
    }
}
