using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrenLib;
using Magic;
using System.Diagnostics;
using System.IO;
using WrenBot.Types;
using WrenBot.Net;
using WrenBot.Net.ClientStructs;
using WrenBot.Net.ServerStructs;

namespace WrenBot
{
    public partial class Form1 : Form
    {
        public Dictionary<uint, BotClient> Clients = new Dictionary<uint, BotClient>();

        public NewProxy Proxy = null;
        public Form1()
        {
            Form.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Proxy = new NewProxy(2610);
            Proxy.OnConnect += new SerialDelegate(_OnConnect);
            Proxy.OnDisconnect += new SerialDelegate(_OnDisconnect);
            Proxy.OnGameServerConnect += new SerialDelegate(_OnGameEnter);
            Proxy.OnGameEnter += new OnGameLogin(_OnGameLogin);
            Proxy.OnRecv += new OnPacketEvent(_RecvHandler);
            Proxy.OnSend += new OnPacketEvent(_SendHandler);

        }

        #region PacketHandlers
        public void _RecvHandler(ProxySocket Socket, uint Serial, NewProxy Proxy, ref MemoryStream Buffer)
        {
            Packet Packet = new Packet(Buffer.ToArray());
            try
            {
                switch ((ServerAction)Packet.Action)
                {
                    #region Location
                    case ServerAction.Location:
                        {
                            PlayerLocation Loc = Packet.Read<PlayerLocation>(0);
                            Clients[Serial].Aisling.Location.X = Loc.X;
                            Clients[Serial].Aisling.Location.Y = Loc.Y;
                            Clients[Serial].Aisling.LastRefresh = DateTime.Now;
                        } break;
                    #endregion
                    #region Map Info
                    case ServerAction.MapInfo:
                        {
                            MapInfo Info = Packet.Read<MapInfo>(0);
                            Clients[Serial].Aisling.Location.Map = Info.MapNumber;
                            if (Clients[Serial].Aisling.Map.Number != Info.MapNumber)
                                Clients[Serial].Aisling.Map.Entities.Clear();
                            Clients[Serial].Aisling.Map.Number = Info.MapNumber;
                            Clients[Serial].Aisling.Map.Width = Info.TileX;
                            Clients[Serial].Aisling.Map.Height = Info.TileY;
                            new System.Threading.Thread(new System.Threading.ThreadStart(Clients[Serial].WaitForMapLoad)).Start();
                            Clients[Serial].Aisling.LastRefresh = DateTime.Now;
                        } break;
                    #endregion
                    #region Animation
                    case ServerAction.Animation:
                        {
                            string PString = Packet.GetPacketString(Packet.Data);
                            Net.ServerStructs.Animation AnimationStruct = Packet.Read<Net.ServerStructs.Animation>(0);
                            Types.Animation Animation = new Types.Animation(AnimationStruct.ToWho, AnimationStruct.FromWho, AnimationStruct.Number, AnimationStruct.Speed);
                            Clients[Serial].Aisling.Map.Entities[Animation.ToWho].Animations.Add(Animation);
                            if (Clients[Serial].Aisling.Map.Entities[Animation.ToWho].Animations.Count > 30)
                            {
                                Clients[Serial].Aisling.Map.Entities[Animation.ToWho].Animations =
                                    new List<Types.Animation>(
                                        from var
                                        in Clients[Serial].Aisling.Map.Entities[Animation.ToWho].Animations
                                        where var.TimeElapsed < new TimeSpan(0, 5, 0)
                                        select var
                                    );
                            }
                            if (AnimationStruct.ToWho == Clients[Serial].Aisling.Serial)
                                Clients[Serial].Aisling.Animations.Add(Animation);
                            else
                            {
                                if (Clients[Serial].Aisling.Map.Entities.ContainsKey(Animation.ToWho) &&
                                    Clients[Serial].Aisling.Map.Entities.ContainsKey(Animation.FromWho))
                                {
                                    if (Clients[Serial].Aisling.Map.Entities[Animation.FromWho].EntityType == MapEntity.Type.Player)
                                    {
                                        if (Clients[Serial].Aisling.Map.Entities[Animation.ToWho].EntityType == MapEntity.Type.Monster)
                                        {
                                            if (!(Clients[Serial].Aisling.Map.Entities[Animation.FromWho] as AislingEntity).Targets.Contains(Animation.ToWho))
                                                (Clients[Serial].Aisling.Map.Entities[Animation.FromWho] as AislingEntity).Targets.Add(Animation.ToWho);
                                        }
                                        foreach (MapEntity Entity in Clients[Serial].Aisling.Map.FindEntities((MapEntity MapEnt) => MapEnt.EntityType == MapEntity.Type.Monster && MapEnt.Serial == Animation.ToWho))
                                            (Entity as Monster).Animations.Add(Animation);
                                        (Clients[Serial].Aisling.Map.Entities[Animation.FromWho] as AislingEntity).Targets =
                                            new List<uint>
                                            (
                                                from var in (Clients[Serial].Aisling.Map.Entities[Animation.FromWho] as AislingEntity).Targets
                                                where Clients[Serial].Aisling.Map.Entities.ContainsKey(var)
                                                select var
                                            );
                                    }
                                }
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Skill Slot Info
                    case ServerAction.SkillSlotInfo:
                        {
                            SkillSlotInfo Info = Packet.Read<SkillSlotInfo>(0);
                            if (Info.Slot <= 36)
                            {
                                Clients[Serial].Aisling.TemSkills[(int)(Info.Slot - 1)] = new SkillSlot()
                                {
                                    Icon = Info.Icon,
                                    Name = Info.Name
                                };
                            }
                            else if (Info.Slot <= 72)
                            {
                                Clients[Serial].Aisling.MedSkills[(int)(Info.Slot - 37)] = new SkillSlot()
                                {
                                    Icon = Info.Icon,
                                    Name = Info.Name
                                };
                            }
                            else if (Info.Slot <= 90)
                            {
                                Clients[Serial].Aisling.WorldSkills[(int)(Info.Slot - 73)] = new SkillSlot()
                                {
                                    Icon = Info.Icon,
                                    Name = Info.Name
                                };
                            }
                        } break;
                    #endregion
                    #region Spell Slot Info
                    case ServerAction.SpellSlotInfo:
                        {
                            SpellSlotInfo Info = Packet.Read<SpellSlotInfo>(0);
                            if (Info.Slot <= 36)
                            {
                                Clients[Serial].Aisling.TemSpells[Info.Slot - 1] = new SpellSlot()
                                {
                                    Lines = Info.Lines,
                                    Name = Info.Name,
                                    Prompt = Info.Prompt,
                                    TargetType = Info.TargetType
                                };
                            }
                            else if (Info.Slot <= 72)
                            {
                                Clients[Serial].Aisling.MedSpells[Info.Slot - 37] = new SpellSlot()
                                {
                                    Lines = Info.Lines,
                                    Name = Info.Name,
                                    Prompt = Info.Prompt,
                                    TargetType = Info.TargetType
                                };
                            }
                            else if (Info.Slot <= 90)
                            {
                                Clients[Serial].Aisling.WorldSpells[Info.Slot - 73] = new SpellSlot()
                                {
                                    Lines = Info.Lines,
                                    Name = Info.Name,
                                    Prompt = Info.Prompt,
                                    TargetType = Info.TargetType
                                };
                            }
                        } break;
                    #endregion
                    #region Remove Slot
                    case ServerAction.RemoveItem:
                        {
                            RemoveSlot Info = Packet.Read<RemoveSlot>(0);
                            Clients[Serial].Aisling.Inventory[Info.Slot - 1] = null;
                        } break;
                    case ServerAction.RemoveSpell:
                        {
                            RemoveSlot Info = Packet.Read<RemoveSlot>(0);
                            if (Info.Slot <= 36)
                                Clients[Serial].Aisling.TemSpells[(int)(Info.Slot - 1)] = null;
                            else if (Info.Slot <= 72)
                                Clients[Serial].Aisling.MedSpells[(int)(Info.Slot - 37)] = null;
                            else if (Info.Slot <= 90)
                                Clients[Serial].Aisling.WorldSpells[(int)(Info.Slot - 73)] = null;
                        } break;
                    case ServerAction.RemoveSkill:
                        {
                            RemoveSlot Info = Packet.Read<RemoveSlot>(0);
                            if (Info.Slot <= 36)
                                Clients[Serial].Aisling.TemSkills[(int)(Info.Slot - 1)] = null;
                            else if (Info.Slot <= 72)
                                Clients[Serial].Aisling.MedSkills[(int)(Info.Slot - 37)] = null;
                            else if (Info.Slot <= 90)
                                Clients[Serial].Aisling.WorldSkills[(int)(Info.Slot - 73)] = null;
                        } break;
                    #endregion
                    #region Spell Bar
                    case ServerAction.SpellBar:
                        {
                            PlayerSpellBar PlayerBar = Packet.Read<PlayerSpellBar>(0);
                            Types.SpellBar SpellBar = new SpellBar(PlayerBar.Icon, PlayerBar.Color);
                            if (!Clients[Serial].Aisling.SpellBar.ContainsKey(PlayerBar.Icon))
                                Clients[Serial].Aisling.SpellBar.Add(PlayerBar.Icon, SpellBar);
                            if (SpellBar.Color == PlayerSpellBar.SpellIconColor.Gone)
                                Clients[Serial].Aisling.SpellBar.Remove(SpellBar.Icon);
                        } break;
                    #endregion
                    #region Client Walk
                    case ServerAction.ClientWalk:
                        {
                            ClientSpriteWalk WalkingDirection = Packet.Read<ClientSpriteWalk>(0);
                            Clients[Serial].Aisling.Location.AbsX = WalkingDirection.X;
                            Clients[Serial].Aisling.Location.AbsY = WalkingDirection.Y;
                            switch (WalkingDirection.Direction)
                            {
                                case FaceDirection.Down:
                                    Clients[Serial].Aisling.Location.AbsY++;
                                    break;
                                case FaceDirection.Up:
                                    Clients[Serial].Aisling.Location.AbsY--;
                                    break;
                                case FaceDirection.Left:
                                    Clients[Serial].Aisling.Location.AbsX--;
                                    break;
                                case FaceDirection.Right:
                                    Clients[Serial].Aisling.Location.AbsX++;
                                    break;
                            }
                            Clients[Serial].Aisling.Location.AbsLocation = WalkingDirection.Direction;
                            Clients[Serial].Aisling.LastBattle = DateTime.Now;
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region HPBar Display
                    case ServerAction.DisplayHPBAR:
                        {
                            HPBAR EntityBar = Packet.Read<HPBAR>(0);
                            Console.WriteLine(EntityBar.Percent);
                            if (Clients[Serial].Aisling.Map.Entities.ContainsKey(EntityBar.Serial))
                                Clients[Serial].Aisling.Map.Entities[EntityBar.Serial].HPPercent = EntityBar.Percent;
                            if (EntityBar.Serial == Clients[Serial].Aisling.Serial)
                            {
                                Clients[Serial].HPPercent = EntityBar.Percent;
                            }
                            foreach (MapEntity Entity in Clients[Serial].Aisling.Map.FindEntities((MapEntity MapEnt) => MapEnt.EntityType == MapEntity.Type.Monster && MapEnt.Serial == EntityBar.Serial))
                            {
                                try
                                {
                                    if (EntityBar.Percent < 100)
                                        (Entity as Monster).WasHit = true;
                                    (Clients[Serial].EntityFromSerial(EntityBar.Serial) as Monster).WasPramhed = false;
                                    (Clients[Serial].EntityFromSerial(EntityBar.Serial) as Monster).HPPercent = EntityBar.Percent;
                                }
                                catch { }
                            }
                            //if we hit our target, reset hit count to zero
                            if (Clients[Serial].Aisling.testSer == EntityBar.Serial)
                            {
                                Clients[Serial].Aisling.test = 0;
                                //we hit it, so it's not pramhed
                                Clients[Serial].Aisling.Map.Entities[EntityBar.Serial].WasPramhed = false;
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Adding Sprites / Monsters / Npcs / Items
                    case ServerAction.AddSprites:
                        {
                            AddSprites Sprites = AddSprites.FromPacket(Packet);
                            foreach (AddSprites.MonsterSprite Monster in Sprites.Monsters)
                            {
                                if (!Clients[Serial].Aisling.Map.Entities.ContainsKey(Monster.Serial))
                                {
                                    Clients[Serial].Aisling.Map.Entities.Add(Monster.Serial, new Monster()
                                    {
                                        Icon = Monster.Icon,
                                        IsPet = Monster.IsPet,
                                        HPPercent = 100,
                                        Date = DateTime.Now,
                                        Location = new Location()
                                        {
                                            X = Monster.X,
                                            Y = Monster.Y,
                                            Direction = Monster.Direction,
                                            Map = Clients[Serial].Aisling.Location.Map
                                        },
                                        Serial = Monster.Serial
                                    }
                                    );
                                }
                            }
                            foreach (AddSprites.NPCSprite NPC in Sprites.NPCs)
                            {
                                if (Clients[Serial].Aisling.Map.Entities.ContainsKey(NPC.Serial))
                                    Clients[Serial].Aisling.Map.Entities.Remove(NPC.Serial);
                                Clients[Serial].Aisling.Map.Entities.Add(NPC.Serial, new NPC()
                                {
                                    Icon = NPC.Icon,
                                    Location = new Location()
                                    {
                                        X = NPC.X,
                                        Y = NPC.Y,
                                        Direction = NPC.Direction,
                                        Map = Clients[Serial].Aisling.Location.Map
                                    },
                                    Name = NPC.Name,
                                    Serial = NPC.Serial
                                }
                                );
                            }
                            foreach (AddSprites.ItemSprite Item in Sprites.Items)
                            {
                                if (Clients[Serial].Aisling.Map.Entities.ContainsKey(Item.Serial))
                                    Clients[Serial].Aisling.Map.Entities.Remove(Item.Serial);
                                Clients[Serial].Aisling.Map.Entities.Add(Item.Serial, new Item()
                                {
                                    Icon = Item.Icon,
                                    Location = new Location()
                                    {
                                        X = Item.X,
                                        Y = Item.Y,
                                        Direction = (FaceDirection)FaceDirection.Up,
                                        Map = Clients[Serial].Aisling.Location.Map,
                                    },
                                    Serial = Item.Serial
                                }
                                );
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Adding Players
                    case ServerAction.AddPlayer:
                        {
                            if (Packet.Data[11] == 0x00 &&
                                Packet.Data[12] == 0x00 &&
                                Packet.Data[13] == 0x00 &&
                                Packet.Data[14] == 0x00)
                            {
                                Packet.Data[13] = 0x50;
                            }
                            if (Packet[11] == 0xFF && Packet[12] == 0xFF)
                            {
                                ShowPlayerForm Info = Packet.Read<ShowPlayerForm>(0);

                                if (Packet.Data[11] == 0x00 &&
                                    Packet.Data[12] == 0x00 &&
                                    Packet.Data[13] == 0x00 &&
                                    Packet.Data[14] == 0x00)
                                {
                                    Info.Name = "icube";
                                }
                                if (!(new Location() { X = Info.X, Y = Info.Y }.OnScreenOf(Clients[Serial].Aisling.Location)))
                                    break;
                                if (Info.Serial == Clients[Serial].Aisling.Serial)
                                {
                                    Clients[Serial].Aisling.Location.X = Info.X;
                                    Clients[Serial].Aisling.Location.Y = Info.Y;
                                }
                                else
                                {
                                    if (Clients[Serial].Aisling.Map.Entities.ContainsKey(Info.Serial))
                                        Clients[Serial].Aisling.Map.Entities.Remove(Info.Serial);
                                    Clients[Serial].Aisling.Map.Entities.Add(Info.Serial, new AislingEntity()
                                    {
                                        LegendInfo = new LegendInfo.Entry[0],
                                        Location = new Location()
                                        {
                                            X = Info.X,
                                            Y = Info.Y,
                                            Map = Clients[Serial].Aisling.Location.Map,
                                            Direction = Info.Direction
                                        },
                                        Name = Info.Name.value,
                                        Serial = Info.Serial,
                                        HoverMessage = Info.HoverMessage.value
                                    }
                                    );
                                }
                            }
                            else
                            {
                                ShowPlayer Info = null;
                                Info = Packet.Read<ShowPlayer>(0);
                                Console.WriteLine(Info.Name);
                                if (!(new Location() { X = Info.X, Y = Info.Y }.OnScreenOf(Clients[Serial].Aisling.Location)))
                                    break;
                                if (Packet.Data[11] == 0x00 &&
                                    Packet.Data[12] == 0x00 &&
                                    Packet.Data[13] == 0x00 &&
                                    Packet.Data[14] == 0x00)
                                {
                                    Info.Name = "icube";
                                }
                                if (Info.Serial == Clients[Serial].Aisling.Serial)
                                {
                                    Clients[Serial].Aisling.Location.X = Info.X;
                                    Clients[Serial].Aisling.Location.Y = Info.Y;
                                }
                                else
                                {
                                    if (Clients[Serial].Aisling.Map.Entities.ContainsKey(Info.Serial))
                                        Clients[Serial].Aisling.Map.Entities.Remove(Info.Serial);
                                    Clients[Serial].Aisling.Map.Entities.Add(Info.Serial, new AislingEntity()
                                    {
                                        LegendInfo = new LegendInfo.Entry[0],
                                        Location = new Location()
                                        {
                                            X = Info.X,
                                            Y = Info.Y,
                                            Map = Clients[Serial].Aisling.Location.Map,
                                            Direction = Info.Direction
                                        },
                                        Name = Info.Name.value,
                                        Serial = Info.Serial,
                                        HoverMessage = Info.HoverMessage.value
                                    }
                                    );
                                }
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Remove Sprite
                    case ServerAction.RemoveSprite:
                        {
                            uint EntitySerial = Packet.Read<RemoveSprite>(0).ID;
                            if (Clients[Serial].Aisling.Map.Entities.ContainsKey(EntitySerial))
                                Clients[Serial].Aisling.Map.Entities.Remove(EntitySerial);

                        } break;
                    #endregion
                    #region Stats Updated
                    case ServerAction.StatsUpdated:
                        {
                            bool[] Bools = Packet.Read<StatsUpdate>(0).BitMaskValues();
                            int Index = 3;
                            if (Bools[2])
                            {
                                StatsStructA StructA = Packet.Read<StatsStructA>(Index);
                                Clients[Serial].Aisling.Stats.STR = StructA.STR;
                                Clients[Serial].Aisling.Stats.INT = StructA.INT;
                                Clients[Serial].Aisling.Stats.WIS = StructA.WIS;
                                Clients[Serial].Aisling.Stats.CON = StructA.CON;
                                Clients[Serial].Aisling.Stats.DEX = StructA.DEX;
                                Clients[Serial].Aisling.Stats.Points = StructA.Points;
                                Clients[Serial].Aisling.Stats.MaxHP = StructA.HPMax;
                                Clients[Serial].Aisling.Stats.MaxMP = StructA.MPMax;
                                Clients[Serial].Aisling.Stats.WeightCurr = StructA.WeightCurr;
                                Clients[Serial].Aisling.Stats.WeightMax = StructA.WeightMax;
                                Clients[Serial].Aisling.Stats.Ability = StructA.Ability;
                                Clients[Serial].Aisling.Stats.Level = StructA.Level;
                                Index += 28;
                            }
                            if (Bools[3])
                            {
                                StatsStructB StructB = Packet.Read<StatsStructB>(Index);
                                Clients[Serial].Aisling.Stats.CurHP = StructB.HPCurr;
                                Clients[Serial].Aisling.Stats.CurMP = StructB.MPCurr;
                                Index += 8;
                            }
                            if (Bools[4])
                            {
                                StatsStructC StructC = Packet.Read<StatsStructC>(Index);
                                Clients[Serial].Aisling.Stats.AbilityExperience = StructC.AExp;
                                Clients[Serial].Aisling.Stats.LevelExperience = StructC.EXP;
                                Clients[Serial].Aisling.Stats.Gold = StructC.Gold;
                                Clients[Serial].Aisling.Stats.GP = StructC.GP;
                                Clients[Serial].Aisling.Stats.NextLevel = StructC.NextLev;
                                Clients[Serial].Aisling.Stats.NextAbility = StructC.NextAB;
                                Index += 24;
                            }
                            if (Bools[5])
                            {
                                StatsStructD StructD = Packet.Read<StatsStructD>(Index);
                                Clients[Serial].Aisling.Stats.AC = StructD.AC;
                                Clients[Serial].Aisling.Stats.MR = StructD.MR;
                                Clients[Serial].Aisling.Stats.HIT = StructD.HIT;
                                Clients[Serial].Aisling.Stats.DAM = StructD.DAM;
                                Clients[Serial].Aisling.Stats.AttackElement = StructD.AEle;
                                Clients[Serial].Aisling.Stats.DefenseElement = StructD.DEle;
                                Index += 13;
                            }
                        }
                        break;
                    #endregion
                    #region Entity Walked
                    case ServerAction.EntityWalked:
                        {
                            SpriteWalk EntityWalk = Packet.Read<SpriteWalk>(0);
                            ushort XDIFF = EntityWalk.X, YDIFF = EntityWalk.Y;
                            switch (EntityWalk.Direction)
                            {
                                case FaceDirection.Down:
                                    YDIFF++;
                                    break;
                                case FaceDirection.Left:
                                    XDIFF--;
                                    break;
                                case FaceDirection.Right:
                                    XDIFF++;
                                    break;
                                case FaceDirection.Up:
                                    YDIFF--;
                                    break;
                            }
                            if (Clients[Serial].Aisling.Map.Entities.ContainsKey(EntityWalk.Serial))
                            {
                                if (Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial].EntityType == MapEntity.Type.Monster)
                                    (Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial] as Monster).WasHit = false;
                                Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial].WasHit = false;
                                Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial].Location.X = XDIFF;
                                Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial].Location.Y = YDIFF;
                                Clients[Serial].Aisling.Map.Entities[EntityWalk.Serial].Location.Direction = EntityWalk.Direction;
                            }
                            if (Clients[Serial].AttackTargetSerial == EntityWalk.Serial)
                            {
                                Clients[Serial].Aisling.AttackLoops = 0;
                                Clients[Serial].Aisling.test2 = false;
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Appendage
                    case ServerAction.Appendage:
                        {
                            Appendage BodyItems = Packet.Read<Appendage>(0);
                            Clients[Serial].Aisling.Body[(Aisling.SpriteBody.Appendage.Slot)BodyItems.Slot] = new Aisling.SpriteBody.Appendage(BodyItems.Icon, BodyItems.Name);
                            Clients[Serial].Aisling.Body[(Aisling.SpriteBody.Appendage.Slot)BodyItems.Slot].Icon = BodyItems.Icon;
                        } break;
                    #endregion
                    #region Entity Turned
                    case ServerAction.EntityTurn:
                        {
                            EntityDirection EntityDir = Packet.Read<EntityDirection>(0);
                            if (Clients[Serial].Aisling.Map.Entities.ContainsKey(EntityDir.Serial))
                            {
                                Clients[Serial].Aisling.Map.Entities[EntityDir.Serial].Location.Direction = EntityDir.FaceDirection;
                                if (Clients[Serial].Aisling.Map.Entities[EntityDir.Serial].EntityType == MapEntity.Type.Monster)
                                    (Clients[Serial].Aisling.Map.Entities[EntityDir.Serial] as Monster).WasSuained = false;
                            }
                            List<uint> SerialsToRemove = new List<uint>();
                            foreach (KeyValuePair<uint, MapEntity> KVP in Clients[Serial].Aisling.Map.Entities)
                                if (!KVP.Value.Location.OnScreenOf(Clients[Serial].Aisling.Location) || KVP.Value.HPPercent == 0)
                                    SerialsToRemove.Add(KVP.Value.Serial);
                            foreach (uint Ser in SerialsToRemove)
                            {
                                Clients[Serial].Aisling.Map.Entities.Remove(Ser);
                            }
                        } break;
                    #endregion
                    #region Body Animation
                    case ServerAction.BodyAnimation:
                        {
                            SpriteAnimation EntityAnimation = Packet.Read<SpriteAnimation>(0);
                            if (Clients[Serial].Aisling.Serial == EntityAnimation.ID)
                                Clients[Serial].Aisling.AttackLanded = true;
                            else Clients[Serial].Aisling.AttackLanded = false;

                            //we enganged attacking
                            if (EntityAnimation.ID == Clients[Serial].Aisling.Serial)
                            {
                                if (EntityAnimation.Animation == 1 || EntityAnimation.Animation == 129 ||
                                    EntityAnimation.Animation == 139 || EntityAnimation.Animation == 132)
                                {
                                    Clients[Serial].Aisling.Swings++;
                                    Clients[Serial].Aisling.EngagedCombat = true;
                                }
                                else Clients[Serial].Aisling.EngagedCombat = false;
                            }
                            else
                            {
                                Clients[Serial].Aisling.EngagedCombat = false;
                                if (Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID].EntityType == MapEntity.Type.Player)
                                {
                                    var v =
                                        from var
                                        in Clients[Serial].Aisling.Map.EntityList
                                        where
                                            var.Location.X == Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID].Location.InfrontOf.X &&
                                            var.Location.Y == Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID].Location.InfrontOf.Y
                                        select var;
                                    if (v.Count() > 0)
                                    {
                                        foreach (var ent in v)
                                            if (!(Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID] as AislingEntity).Targets.Contains(ent.Serial))
                                                (Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID] as AislingEntity).Targets.Add(ent.Serial);
                                    }
                                    (Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID] as AislingEntity).Targets =
                                            new List<uint>
                                            (
                                                from var in (Clients[Serial].Aisling.Map.Entities[EntityAnimation.ID] as AislingEntity).Targets
                                                where Clients[Serial].Aisling.Map.Entities.ContainsKey(var)
                                                select var
                                            );
                                }
                            }
                        } break;
                    #endregion
                    #region Sound Played
                    case ServerAction.SoundPlay:
                        {
                            PlaySound SoundNumber = Packet.Read<PlaySound>(0);
                            Clients[Serial].Aisling.LastSound = new Sounds() { Number = SoundNumber.Number };
                            if (Clients[Serial].AttackTargetSerial > 0)
                                Clients[Serial].Aisling.testSer = Clients[Serial].AttackTargetSerial;
                        } break;
                    #endregion
                    #region Chat Messages
                    case ServerAction.Chat:
                        {
                            Chat ChatMessage = Packet.Read<Chat>(0);
                            Clients[Serial].Aisling.ChatMessages.Add(new Aisling.ChatMessage(Convert.ToBoolean(ChatMessage.Type), ChatMessage.Serial, ChatMessage.Message));
                        } break;
                    #endregion
                    #region Bar Messages
                    case ServerAction.Bar:
                        {
                            BarMessage BarMessage = Packet.Read<BarMessage>(0);
                            Clients[Serial].Aisling.BarMessages.Add(new Aisling.BarMessage((Aisling.BarMessage.MessageType)BarMessage.Type, BarMessage.Message));
                            if (BarMessage.Message.value.ToLower().StartsWith("these items are cursed"))
                                if (Clients[Serial].Aisling.Map.Entities.ContainsKey(Clients[Serial].ItemTargetSerial))
                                    (Clients[Serial].Aisling.Map.Entities[Clients[Serial].ItemTargetSerial] as Item).IsBanned = true;
                        } break;
                    #endregion
                    #region Group
                    case ServerAction.GroupRequest:
                        {
                            GroupRequest Request = Packet.Read<GroupRequest>(0);
                        } break;
                    #endregion
                    #region Remove Appendage
                    case ServerAction.RemoveAppendage:
                        {
                            if (Clients[Serial].Aisling.Body[(Aisling.SpriteBody.Appendage.Slot)Packet[2]] != null)
                            {
                                string ItemRemoved = Clients[Serial].Aisling.Body[(Aisling.SpriteBody.Appendage.Slot)Packet[2]].Name;
                            }
                            Clients[Serial].Aisling.Body[(Aisling.SpriteBody.Appendage.Slot)Packet[2]] = null;
                        } break;
                    #endregion
                    default:
                        {
                        } break;
                }
                if (Clients[Serial].Aisling.EngagedCombat)
                {
                    Clients[Serial].Aisling.test++;
                    Clients[Serial].Aisling.EngagedCombat = false;
                }
            }
            catch { }
        }

        public void _SendHandler(ProxySocket Socket, uint Serial, NewProxy Proxy, ref MemoryStream Buffer)
        {
            Packet Packet = new Packet(Buffer.ToArray());
            switch ((ClientAction)Packet.Action)
            {
                #region Walking
                case ClientAction.Walking:
                    {
                        Walking WalkingDirection = Packet.Read<Walking>(0);
                        Clients[Serial].Aisling.Location.Direction = WalkingDirection.Direction;
                        switch (Clients[Serial].Aisling.Location.Direction)
                        {
                            case FaceDirection.Down:
                                Clients[Serial].Aisling.Location.Y++;
                                break;
                            case FaceDirection.Up:
                                Clients[Serial].Aisling.Location.Y--;
                                break;
                            case FaceDirection.Left:
                                Clients[Serial].Aisling.Location.X--;
                                break;
                            case FaceDirection.Right:
                                Clients[Serial].Aisling.Location.X++;
                                break;
                        }
                    } break;
                #endregion
                #region Turning
                case ClientAction.Turning:
                    {
                        Direction TurnDirection = Packet.Read<Direction>(0);
                        Clients[Serial].Aisling.Location.Direction = TurnDirection.FaceDirection;
                    } break;
                #endregion
                #region Skill Used
                case ClientAction.UseSkill:
                    {
                        UseSkill SkillInfo = Packet.Read<UseSkill>(0);
                    } break;
                #endregion
                #region Entity Clicked
                case ClientAction.ClickEntity:
                    {
                        ClickEntity Subject = Packet.Read<ClickEntity>(0);
                    } break;
                #endregion
                #region Last Refresh
                case (ClientAction)0x38:
                    {
                        Clients[Serial].Aisling.LastRefresh = DateTime.Now;
                    } break;
                #endregion
                default:
                    {
                    } break;
            }
        }
        #endregion

        public void _OnConnect(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            int BotClients = Clients.Count();
            if (Proxy.Clients.ContainsKey(ClientSerial))
            {
                if (!Clients.ContainsKey(ClientSerial))
                {
                    Clients.Add(ClientSerial, new BotClient(this)
                    {
                        ClientSerial = ClientSerial,
                        Proxy = Proxy,
                        Socket = Socket
                    });
                }
            }
        }

        public BlackMagic Magic;
        public string Character;
        public BlackMagic EnableMagic(string who, ProxySocket Socket)
        {
            Process[] p = Process.GetProcessesByName("Darkages");
            foreach (Process s in p)
            {
                Magic = new BlackMagic(s.MainWindowHandle);
                var o = Magic.ReadASCIIString(0x0075E850, 20);
                if (this.Character.ToLower() == o.ToLower())
                {
                    return Magic;
                }
            }
            return null;
        }

        public void _OnGameLogin(ProxySocket Socket, uint ClientSerial, NewProxy Proxy, string CharacterName)
        {
            this.Character = CharacterName;
            Socket.Name = CharacterName;
            Clients[Socket.ConnectedSocket.ID].Aisling.Name = CharacterName;
            Clients[ClientSerial].Roles = new WrenBot.Types.ClientRoles()
            {
                CharacterName = CharacterName,
                Role = WrenBot.Types.ClientRoles.Roles.NonSet,
                Socket = Socket
            };
            ListViewItem I = new ListViewItem(new string[] { Clients[ClientSerial].Roles.CharacterName, Clients[ClientSerial].Roles.Role.ToString(), "Idle" });
            listView1.Items.Add(I);
        }

        public void _OnDisconnect(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            try
            {
                if (Clients[ClientSerial].Magic.ReadByte(0x00780488) == 1)
                {
                    if (Proxy.Clients.ContainsKey(ClientSerial))
                    {
                        Proxy.Clients.Remove(ClientSerial);
                        listView1.Items.Remove(GetItemFromSocket(Socket));
                    }
                    Clients.Remove(ClientSerial);
                }
            }
            catch
            {
            }
            RemoveUnwantedItems();
        }

        public void _OnGameEnter(ProxySocket Socket, uint ClientSerial, NewProxy Proxy)
        {
            BlackMagic M = EnableMagic(Socket.Name, Socket); 
            Clients[ClientSerial].Magic = M;
        }

        public ListViewItem GetItemFromSocket(ProxySocket Socket)
        {
            foreach (ListViewItem s in listView1.Items)
            {
                if (Socket.Name.ToLower() == s.SubItems[0].Text.ToLower())
                {
                    return s;
                }
            }
            return null;
        }

        public void RemoveUnwantedItems()
        {
            foreach (ListViewItem f in listView1.Items)
            {
                string n = f.SubItems[0].Text.ToLower();
                try
                {
                    var Socket = (from v in Proxy.Clients
                                  where v.Value.Name.ToLower() == n.ToLower()
                                  select v.Value).Single();
                }
                catch
                {
                    listView1.Items.Remove(f);
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var i = (ListView)sender;
                string n = listView1.SelectedItems[0].SubItems[0].Text;
                var Socket = (from v in Proxy.Clients
                              where v.Value.Name.ToLower() == n.ToLower()
                              select v.Value).Single();
                if (Socket != null)
                {
                    if (!Clients[Socket.ConnectedSocket.ID].RolesForm.Visible)
                    {
                        Clients[Socket.ConnectedSocket.ID].RolesForm.Socket = Socket;
                        Invoke(new MethodInvoker(Clients[Socket.ConnectedSocket.ID].RolesForm.Show));
                    }
                    else
                    {
                        Invoke(new MethodInvoker(Clients[Socket.ConnectedSocket.ID].RolesForm.Hide));
                    }
                }
            }
            catch 
            {
            }
        }
    }
}
