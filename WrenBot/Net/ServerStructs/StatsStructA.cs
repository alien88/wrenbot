using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class StatsStructA
    {
        public ushort Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte Level { get; set; }
        public byte Ability { get; set; }
        public uint HPMax { get; set; }
        public uint MPMax { get; set; }
        public byte STR { get; set; }
        public byte INT { get; set; }
        public byte WIS { get; set; }
        public byte CON { get; set; }
        public byte DEX { get; set; }
        public byte Flasher { get; set; }
        public byte Points { get; set; }
        public ushort WeightMax { get; set; }
        public ushort WeightCurr { get; set; }
        public uint Unknown3 { get; set; }
    }
}
