using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class PlayerSpellBar
    {
        public enum SpellIconColor
        {
            White = 0x06,
            Red = 0x05, 
            Orange = 0x04,
            Yellow = 0x03,
            Green = 0x02,
            Blue = 0x01,
            Gone = 0x00
        }
        public byte Action { get { return 0x3A; } set { } }
        public byte Ordinal { get; set; }
        public ushort Icon { get; set; }
        public SpellIconColor Color { get; set; }
    }
}
