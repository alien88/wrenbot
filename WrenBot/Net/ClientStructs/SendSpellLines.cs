using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    class SendSpellLines
    {
        public byte Action { get { return 0x4D; } set { } }
        public byte Ordinal { get; set; }
        public byte Lines { get; set; }
    }
}
