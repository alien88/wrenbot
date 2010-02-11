using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class CountryList
    {
        public CountryList()
        {
            Users = new UserInfo[0];
        }
        public byte Action { get { return 0x36; } set { } }
        public byte Ordinal { get; set; }
        public ushort Total { get; set; }
        public ushort Listed { get; set; }
        public class UserInfo
        {
            public byte unknownA { get; set; }
            public byte Color { get; set; }
            public byte StatusIcon { get; set; }
            public string8 Title { get; set; }
            public bool IsMaster { get; set; }
            public string8 Name { get; set; }
        }
        public UserInfo[] Users { get; set; }
    }
}
