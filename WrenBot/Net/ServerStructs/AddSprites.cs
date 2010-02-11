using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Types;

namespace WrenBot.Net.ServerStructs
{
    public class AddSprites
    {
        public byte Action { get { return 0x07; } set { } }
        public byte Ordinal { get; set; }
        public ushort NumEntities { get { return (ushort)(NPCs.Length + Monsters.Length + Items.Length); } set { } }
        public class NPCSprite
        {
            public ushort X { get; set; }
            public ushort Y { get; set; }
            public uint Serial { get; set; }
            public ushort Icon { get; set; }
            public byte[] WE { get { return new byte[] { 0x00, 0x00, 0x00, 0x00 }; } set { } }
            public FaceDirection Direction { get; set; }
            public byte[] WE2 { get { return new byte[] { 0x00, 0x02 }; } set { } }
            public string8 Name { get; set; }
        }
        public NPCSprite[] NPCs { get; set; }
        public class MonsterSprite
        {
            public bool IsPet;
            public ushort X { get; set; }
            public ushort Y { get; set; }
            public uint Serial { get; set; }
            public ushort Icon { get; set; }
            public byte[] WE { get { return new byte[] { 0x00, 0x00, 0x00, 0x00 }; } set { } }
            public FaceDirection Direction { get; set; }
            public byte[] WE2 { get { return new byte[] { 0x00, 0x00 }; } set { } }
        }
        public MonsterSprite[] Monsters { get; set; }
        public class ItemSprite
        {
            public ushort X { get; set; }
            public ushort Y { get; set; }
            public uint Serial { get; set; }
            public ushort Icon { get; set; }
            public byte[] WE { get { return new byte[] { 0x00, 0x00, 0x00 }; } set { } }
        }
        public ItemSprite[] Items { get; set; }

        public static AddSprites FromPacket(Packet Packet)
        {
            AddSprites Object = new AddSprites()
            {
                Ordinal = Packet.Ordinal
            };
            ushort Len = (ushort)((Packet[2] << 8) + Packet[3]);
            List<ItemSprite> Items = new List<ItemSprite>();
            List<MonsterSprite> Monsters = new List<MonsterSprite>();
            List<NPCSprite> NPCs = new List<NPCSprite>();
            int Index = 4;
            for (int i = 0; i < Len; i++)
            {
                ushort X = (ushort)((Packet[Index] << 8) + Packet[Index + 1]);
                ushort Y = (ushort)((Packet[Index + 2] << 8) + Packet[Index + 3]);
                uint Serial = (uint)((Packet[Index + 4] << 24) + (Packet[Index + 5] << 16) + (Packet[Index + 6] << 8) + Packet[Index + 7]);
                ushort Icon = (ushort)((Packet[Index + 8] << 8) + Packet[Index + 9]);
                byte[] WE1 = new byte[] { Packet[Index + 10], Packet[Index + 11], Packet[Index + 12] };
                if (Icon > 0x8000 && Icon < 0x9000)
                {
                    Items.Add(new ItemSprite()
                    {
                        Icon = Icon,
                        Serial = Serial,
                        X = X,
                        Y = Y
                    }
                    );
                    Index += 13;
                }
                else
                {
                    FaceDirection Direction = (FaceDirection)Packet[Index + 14];
                    byte[] WE2 = new byte[] { Packet[Index + 13], Packet[Index + 14], Packet[Index + 15], Packet[Index + 16] };
                    if (WE2[3] == 0x00 || WE2[3] == 0x01)
                    {
                        Monsters.Add(new MonsterSprite()
                        {
                            IsPet = WE2[3] == 0x01 ? true : false,
                            Icon = Icon,
                            Serial = Serial,
                            X = X,
                            Y = Y
                        }
                        );
                        Index += 17;
                    }
                    else
                    {
                        try
                        {
                            string Name = Encoding.ASCII.GetString(Packet.Data, Index + 18, (int)(Packet[Index + 17]));
                            NPCs.Add(new NPCSprite()
                            {
                                Direction = Direction,
                                Icon = Icon,
                                Name = Name,
                                Serial = Serial,
                                X = X,
                                Y = Y
                            }
                            );
                            Index += 18 + Name.Length;
                        }
                        catch { }
                    }
                }
            }
            Object.Items = Items.ToArray();
            Object.Monsters = Monsters.ToArray();
            Object.NPCs = NPCs.ToArray();
            return Object;
        }
    }
}