using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    class GroupRequest
    {
        public byte Action { get { return 0x63; } set { } }
        public byte Ordinal { get; set; }
        public byte NFI { get; set; }
        public string8 Name { get; set; }
    }
}
