using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ClientStructs
{
    public class InitiateSpellCast
    {
        public byte Action { get { return 0x4D; } set { } }
        public byte Ordinal { get; set; }
        public byte NumLines { get; set; }
        public byte Nothing { get; set; }
    }
    public class SendSpellLine
    {
        public byte Action { get { return 0x4E; } set { } }
        public byte Ordinal { get; set; }
        public string8 Line { get; set; }
        public byte Nothing { get; set; }
    }
    public class CastSpell
    {
        public byte Action { get { return 0x0F; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public byte Nothing { get; set; }
    }
    public class CastSpellTarget
    {
        public byte Action { get { return 0x0F; } set { } }
        public byte Ordinal { get; set; }
        public byte Slot { get; set; }
        public uint Serial { get; set; }
        public uint NothingA { get { return 0x00340036; } set { } }
        public byte NothingB { get; set; }
    }
}