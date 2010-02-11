using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ClientStructs
{
    class Login
    {
        public byte Action { get { return 0x03; } set { } }
        public byte Ordinal { get; set; }
        public string8 UserName { get; set; }
        public string8 Password { get; set; }
    }
}