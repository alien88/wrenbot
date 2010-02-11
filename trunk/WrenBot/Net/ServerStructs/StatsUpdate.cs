using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class StatsUpdate
    {
        public byte Action { get { return 0x08; } set { } }
        public byte Ordinal { get; set; }
        public byte BitMask { get; set; }
        public bool[] BitMaskValues()
        {
            return new bool[]
            {
                ((BitMask >> 7) % 2) == 1,
                ((BitMask >> 6) % 2) == 1,
                ((BitMask >> 5) % 2) == 1,
                ((BitMask >> 4) % 2) == 1,
                ((BitMask >> 3) % 2) == 1,
                ((BitMask >> 2) % 2) == 1,
                ((BitMask >> 1) % 2) == 1,
                ((BitMask >> 0) % 2) == 1,
            };
        }
    }
}
