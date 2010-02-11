using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    public class ShowPlayer
    {
        public byte Action { get { return 0x33; } set { } }
        public byte Ordinal { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public FaceDirection Direction { get; set; }
        public uint Serial { get; set; }
        public ushort HairStyle { get; set; }
        public byte BodyStyle { get; set; }
        public ushort Arms { get; set; }
        public byte Boots { get; set; }
        public ushort Armor { get; set; }
        public byte Shield { get; set; }
        public byte Weapon { get; set; }
        public byte HairColor { get; set; }
        public byte BootColor { get; set; }
        public byte UnknownA { get { return 0x0F; } set { } }
        public byte HeadAccessory { get { return 0x00; } set { } }
        public byte UnknownC { get { return 0x00; } set { } }
        public byte UnknownD { get { return 0x00; } set { } }
        public uint UnknownE { get { return 0x00000000; } set { } }
        public byte UnknownF { get { return 0x00; } set { } }
        public byte UnknownG { get { return 0x00; } set { } }
        public ushort UnknownH { get { return 0x0000; } set { } }
        public ushort UnknownI { get { return 0x0000; } set { } }
        public ushort UnknownA_1 { get { return 0x0000; } set { } }
        public string8 Name { get; set; }
        public string8 HoverMessage { get; set; }
    }
    public class ShowPlayerForm
    {
        public byte Action { get { return 0x33; } set { } }
        public byte Ordinal { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public FaceDirection Direction { get; set; }
        public uint Serial { get; set; }
        public ushort HairStyle { get; set; }
        public byte BodyStyle { get; set; }
        public ushort Arms { get; set; }
        public byte UnknownC { get { return 0x00; } set { } }
        public byte UnknownD { get { return 0x00; } set { } }
        public uint UnknownE { get { return 0x00000000; } set { } }
        public byte UnknownF { get { return 0x00; } set { } }
        public byte UnknownG { get { return 0x00; } set { } }
        public string8 Name { get; set; }
        public string8 HoverMessage { get; set; }
        //33 [00 04] [00 28] [01] [00 06 2E 43] [FF FF] [41] [A9 24] [00] [00] [00 00 00 00] [00] [00] 07 6B 61 69 64 72 69 6B 00 00
    }
}
