using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class Whisper
    {
        public byte Action { get { return 0x19; } set { } }
        public byte Ordinal { get; set; }
        public string8 Target { get; set; }
        public string8 Message { get; set; }
    }
}