using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class Group
    {
        public byte Action { get { return 0x2E; } set { } }
        public byte Ordinage { get; set; }
        public enum Type
        {
            Request = 0x02,
            Accept = 0x03
        }
        public Type Function { get; set; }
        public string8 Name { get; set; }
        public ushort NFI { get { return 0x0000; } set { } }
    }
}