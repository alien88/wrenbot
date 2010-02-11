using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Net.ServerStructs
{
    public class LegendInfo
    {
        public byte Action { get { return 0x34; } set { } }
        public byte Ordinal { get; set; }
        public uint Serial { get; set; }
        public ushort Weapon { get; set; }
        public byte Nothing1 { get { return 0x00; } set { } }
        public ushort Armor { get; set; }
        public byte Nothing2 { get { return 0x00; } set { } }
        public ushort Shield { get; set; }
        public byte Nothing3 { get { return 0x00; } set { } }
        public ushort TopHelmet { get; set; }
        public byte Nothing4 { get { return 0x00; } set { } }
        public ushort Earings { get; set; }
        public byte Nothing5 { get { return 0x00; } set { } }
        public ushort Necklace { get; set; }
        public byte Nothing6 { get { return 0x00; } set { } }
        public ushort LeftRing { get; set; }
        public byte Nothing7 { get { return 0x00; } set { } }
        public ushort RightRing { get; set; }
        public byte Nothing8 { get { return 0x00; } set { } }
        public ushort LeftHand { get; set; }
        public byte Nothing9 { get { return 0x00; } set { } }
        public ushort RightHand { get; set; }
        public byte Nothing10 { get { return 0x00; } set { } }
        public ushort Belt { get; set; }
        public byte Nothing11 { get { return 0x00; } set { } }
        public ushort Greaves { get; set; }
        public byte Nothing12 { get { return 0x00; } set { } }
        public ushort Acessory1 { get; set; }
        public byte Nothing13 { get { return 0x00; } set { } }
        public ushort Boots { get; set; }
        public byte Nothing14 { get { return 0x00; } set { } }
        public ushort OverArmor { get; set; }
        public byte Nothing15 { get { return 0x00; } set { } }
        public ushort Helmet { get; set; }
        public byte Nothing16 { get { return 0x00; } set { } }
        public ushort Acessory2 { get; set; }
        public byte Nothing17 { get { return 0x00; } set { } }
        public byte NFI { get { return 0x00; } set { } }
        public string8 Name { get; set; }
        public byte Unknown { get { return 0x04; } set { } }
        public string8 Title { get; set; }
        public bool AllowGroup { get; set; }
        public string8 GuildRank { get; set; }
        public string8 Class { get; set; }
        public string8 GuildName { get; set; }
        public ushort LegendLength { get; set; }
        public class Entry
        {
            public byte Icon { get; set; }
            public byte Color { get; set; }
            public string8 Key { get; set; }
            public string8 String { get; set; }
        }
        public Entry[] Entries { get; set; }
    }
}
