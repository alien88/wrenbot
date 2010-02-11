using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;

namespace WrenBot.Types
{
    /// <summary>
    /// Aisling Stats Update Event Delegate
    /// </summary>
    /// <param name="Statistics"></param>
    public delegate void StatsUpdateHandler(Aisling.Statistics Statistics);

    /// <summary>
    /// Body Animation Event Delegate
    /// </summary>
    /// <param name="BodySprite">Body Sprite</param>
    public delegate void BodyAnimationHandler(Aisling.SpriteBodyAnimation BodySprite);

    /// <summary>
    /// Chat Message Event Delegate
    /// </summary>
    /// <param name="ChatMessage">Chat Message</param>
    public delegate void ChatMessageHandler(Aisling.ChatMessage ChatMessage);

    /// <summary>
    /// Bar Message Event Delegate
    /// </summary>
    /// <param name="Name">Name</param>
    /// <param name="Serial">Entity Serial Number</param>
    public delegate void BarMessageHandler(string Name, uint Serial);

    /// <summary>
    /// Darkages Aisling Class
    /// </summary>
    public class Aisling : MarshalByRefObject
    {
        /// <summary>
        /// Default Aisling Constructor
        /// </summary>
        public Aisling()
        {
            this.Name = "";
            this.Location = new Location();
            this.Map = new Map();
            this.Serial = 0;
            this.Animations = new List<Animation>();
            this.Inventory = new ItemSlot[59];
            this.TemSkills = new SkillSlot[35];
            this.MedSkills = new SkillSlot[35];
            this.WorldSkills = new SkillSlot[17];
            this.TemSpells = new SpellSlot[35];
            this.MedSpells = new SpellSlot[35];
            this.WorldSpells = new SpellSlot[17];
            this.SpellBar = new Dictionary<ushort,SpellBar>(10);
            this.Stats = new Statistics();
            this.Body = new SpriteBody();
            this.BodyAnimation = new SpriteBodyAnimation(0, 0, 0);
            this.LastSound = new Sounds();
            this.ChatMessages = new List<ChatMessage>();
            this.BarMessages = new List<BarMessage>();
            this.WayPoints = new List<Point>();
            this.CurrentWayPoint = 0;
            this.HasBeenArded = false;
            this.MonsterLocations = new List<MonsterLocation>();
            this.PlayerLocations = new List<PlayerLocation>();
            this.NeedToReDion = true;
            this.HasDioned = false;
            this.LastRefresh = new DateTime(0);
            this.PlayersAllowedNear = new List<string>();
            this.OnlineList = new OnlineListView(0, 0, new OnlineListView.User[0]);
        }

        /// <summary>
        /// Aisling Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Aisling Location
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Boolean: Has Map Loaded Yet?
        /// </summary>
        public bool MapLoaded { get; set; }

        /// <summary>
        /// Current Loaded Map
        /// </summary>
        public Map Map { get; set; }

        /// <summary>
        /// Aisling Serial Number
        /// </summary>
        public uint Serial { get; set; }

        /// <summary>
        /// Aislings List Of Animations Casted On Aisling
        /// </summary>
        public List<Animation> Animations { get; set; }

        /// <summary>
        /// Aisling Inventory Array
        /// </summary>
        public ItemSlot[] Inventory { get; set; }

        /// <summary>
        /// Aisling Temuair Skills Array
        /// </summary>
        public SkillSlot[] TemSkills { get; set; }

        /// <summary>
        /// Aisling Medenian Skills Array
        /// </summary>
        public SkillSlot[] MedSkills { get; set; }

        /// <summary>
        /// Aisling World Skills Array
        /// </summary>
        public SkillSlot[] WorldSkills { get; set; }

        /// <summary>
        /// Aisling Temuair Spells Array
        /// </summary>
        public SpellSlot[] TemSpells { get; set; }

        /// <summary>
        /// Aisling Medenian Spells Array
        /// </summary>
        public SpellSlot[] MedSpells { get; set; }

        /// <summary>
        /// Aisling World Spells Array
        /// </summary>
        public SpellSlot[] WorldSpells { get; set; }

        /// <summary>
        /// Aisling Spell Bar Dictionary (Current Afflictions/Buffs)
        /// </summary>
        public Dictionary<ushort,SpellBar> SpellBar { get; set; }

        /// <summary>
        /// Aisling Stats
        /// </summary>
        public Statistics Stats { get; set; }

        /// <summary>
        /// Aisling Stats Update Event Handler
        /// </summary>
        public StatsUpdateHandler OnStatsChanged;

        /// <summary>
        /// Aisling Sprite Body (Equipment etc.)
        /// </summary>
        public SpriteBody Body;

        /// <summary>
        /// Aisling Body Animation (Not Currently Used)
        /// </summary>
        public SpriteBodyAnimation BodyAnimation;

        /// <summary>
        /// Aisling Body Animation Event Handler
        /// </summary>
        public BodyAnimationHandler OnBodyAnimation;

        /// <summary>
        /// Last Sound Played
        /// </summary>
        public Sounds LastSound { get; set; }

        /// <summary>
        /// Recent Bar Messages
        /// </summary>
        public List<BarMessage> BarMessages;

        /// <summary>
        /// Recent Chat Messages
        /// </summary>
        public List<ChatMessage> ChatMessages;

        /// <summary>
        /// Chat Message Event Handler
        /// </summary>
        public ChatMessageHandler OnChatMessage;

        /// <summary>
        /// Aisling WayPoint List
        /// </summary>
        public List<Point> WayPoints;

        /// <summary>
        /// Aislings Current WayPoint Selected
        /// </summary>
        public int CurrentWayPoint;

        /// <summary>
        /// Boolean: Is Aisling Arded?
        /// </summary>
        public bool HasBeenArded { get; set; }

        /// <summary>
        /// Boolean: Has Attack Hit?
        /// </summary>
        public bool AttackHit { get; set; }

        /// <summary>
        /// Aislings Current Attack Attempts On Enemy
        /// </summary>
        public int AttackAttempt { get; set; }

        /// <summary>
        /// Boolean: Has Attack Landed?
        /// </summary>
        public bool AttackLanded { get; set; }

        /// <summary>
        /// Boolean: Has Aisling Dioned?
        /// </summary>
        public bool HasDioned { get; set; }

        /// <summary>
        /// Boolean: Does Aisling Need Re-Dion?
        /// </summary>
        public bool NeedToReDion { get; set; }

        /// <summary>
        /// Boolean: Does Aisling Need Re-Mor Dion
        /// </summary>
        public bool NeedToReMorDion { get; set; }

        /// <summary>
        /// Aislings Current Attack Tries
        /// </summary>
        public int AttackTries { get; set; }

        /// <summary>
        /// The Date Of The Last Refresh
        /// </summary>
        public DateTime LastRefresh { get; set; }

        /// <summary>
        /// Boolean: Is Aisling Currently Refreshing?
        /// </summary>
        public bool IsRefreshing { get { return DateTime.Now - LastRefresh < new TimeSpan(0, 0, 0, 0, 500); } }

        /// <summary>
        /// Aislings Current Attack Loops
        /// </summary>
        public int AttackLoops { get; set; }

        /// <summary>
        /// Boolean: Has Attack Connected?
        /// </summary>
        public bool AttackConnected { get; set; }

        /// <summary>
        /// List Of Players Allowed Near Durring Combat
        /// </summary>
        public List<string> PlayersAllowedNear;

        /// <summary>
        /// List Of Players Online
        /// </summary>
        public OnlineListView OnlineList;

        #region Attack Logic Variables
        /// <summary>
        /// Attack Logic: Serial Of Attacked Target
        /// </summary>
        public uint AttackedTarget { get; set; }

        /// <summary>
        /// Attack Logic: Boolean: Is In Combat?
        /// </summary>
        public bool InCombat { get; set; }

        /// <summary>
        /// Attack Logic: Boolean: Is Walking To?
        /// </summary>
        public bool WalkingTo { get; set; }

        /// <summary>
        /// Attack Logic: Boolean: Is Looting?
        /// </summary>
        public bool IsLooting { get; set; }

        /// <summary>
        /// Attack Logic: Boolean: Is Engaged In Combat?
        /// </summary>
        public bool EngagedCombat { get; set; }

        /// <summary>
        /// NFI : Probably Useless
        /// </summary>
        public int test;

        /// <summary>
        /// NFI : Probably Useless
        /// </summary>
        public bool test2;

        /// <summary>
        /// NFI : Probably Useless
        /// </summary>
        public int test3;

        /// <summary>
        /// NFI : Probably Useless
        /// </summary>
        public uint testSer;

        /// <summary>
        /// Attack Logic : Number Of Swings On Current Target
        /// </summary>
        public int Swings { get; set; }
        #endregion

        #region Cast Chant
        /// <summary>
        /// Default Spell Chant
        /// </summary>
        public string SpellChant = ".";
        #endregion

        public string[] SkillSet { get; set; }
        public string[] SpellSet { get; set; }
        public string[] ComboSet { get; set; }

        /// <summary>
        /// Probably Not Used Anymore
        /// </summary>
        /// <param name="Stats"></param>
        public void DionProcess(Statistics Stats)
        {
            this.NeedToReDion = false;
            //Commented this out because basher dion wasnt working as intented.
        }

        /// <summary>
        /// Last Cast : Time of Last spell casted.
        /// </summary>
        public DateTime LastCast { get; set; }

        /// <summary>
        /// Last Battle : Time we last was in combat.
        /// </summary>
        public DateTime LastBattle { get; set; }

        #region Collections
        /// <summary>
        /// Distance Sort Entity Delegate
        /// </summary>
        /// <param name="Entity">First Entity</param>
        /// <param name="Entity2">Seccond Entity</param>
        /// <returns>Compared Result</returns>
        private int DistanceSort(MapEntity Entity, MapEntity Entity2)
        {
            if (Entity == null)
            {
                if (Entity2 == null)
                    return 0;
                return -1;
            }
            if (Entity2 == null)
                return 1;
            return Location.DistanceFrom(Entity.Location).CompareTo(Location.DistanceFrom(Entity2.Location));
        }
        /// <summary>
        /// Map Entities On Screen Sorted By Distance
        /// </summary>
        public List<MapEntity> Entities
        {
            get
            {
                List<MapEntity> Entities = Map.EntityList;
                Entities.Sort(DistanceSort);
                return Entities;
            }
        }
        /// <summary>
        /// Aisling Entities On Screen Sorted By Distance
        /// </summary>
        public List<AislingEntity> Players
        {
            get
            {
                try
                {
                    List<MapEntity> Entities = Map.FindEntities((MapEntity o) => o.EntityType == MapEntity.Type.Player);
                    Entities.Sort(DistanceSort);
                    List<AislingEntity> Players = new List<AislingEntity>();
                    foreach (MapEntity Ent in Entities)
                        Players.Add(Ent as AislingEntity);
                    return Players;
                }
                catch { return null; }
            }
        }
        /// <summary>
        /// Monster Entities On Screen Sorted By Distance
        /// </summary>
        public List<Monster> Monsters
        {
            get
            {
                try
                {
                    List<MapEntity> Entities = Map.FindEntities((MapEntity o) => (o != null) && o.EntityType == MapEntity.Type.Monster);
                    Entities.Sort(DistanceSort);
                    List<Monster> Monsters = new List<Monster>();
                    foreach (MapEntity Ent in Entities)
                        if (!Ent.IsBanned)
                            Monsters.Add(Ent as Monster);
                    return Monsters;
                }
                catch { return null; }
            }
        }
        /// <summary>
        /// Item Entities Sorted By Distance
        /// </summary>
        public List<Item> Items
        {
            get
            {
                List<MapEntity> Entities = Map.FindEntities((MapEntity o) => o.EntityType == MapEntity.Type.Item);
                Entities.Sort(DistanceSort);
                List<Item> Items = new List<Item>();
                foreach (MapEntity Ent in Entities)
                    if (!(Ent as Item).IsBanned)
                        Items.Add(Ent as Item);
                return Items;
            }
        }
        /// <summary>
        /// NPC Entities Sorted By Distance
        /// </summary>
        public List<NPC> NPCs
        {
            get
            {
                List<MapEntity> Entities = Map.FindEntities((MapEntity o) => o.EntityType == MapEntity.Type.NPC);
                Entities.Sort(DistanceSort);
                List<NPC> Monsters = new List<NPC>();
                foreach (MapEntity Ent in Entities)
                    NPCs.Add(Ent as NPC);
                return Monsters;
            }
        }
        #endregion

        #region Monster Map Locations
        /// <summary>
        /// Monsters On Screen Location List
        /// </summary>
        public List<MonsterLocation> MonsterLocations;
        public class MonsterLocation
        {
            public ushort X;
            public ushort Y;
        }
        /// <summary>
        /// Players On Screen Location List
        /// </summary>
        public List<PlayerLocation> PlayerLocations;
        public class PlayerLocation
        {
            public ushort X;
            public ushort Y;
        }
        #endregion

        #region Stats
        /// <summary>
        /// Aisling Stats Object
        /// </summary>
        public class Statistics
        {
            /// <summary>
            /// Aisling Ability Level
            /// </summary>
            public byte Ability { get; set; }

            /// <summary>
            /// Aisling Ability Experience
            /// </summary>
            public uint AbilityExperience { get; set; }

            /// <summary>
            /// Aislings Next Ability Experience Needed
            /// </summary>
            public uint NextAbility { get; set; }

            /// <summary>
            /// Aislings Level
            /// </summary>
            public byte Level { get; set; }

            /// <summary>
            /// Aislings Level Experience
            /// </summary>
            public uint LevelExperience { get; set; }

            /// <summary>
            /// Aislings Next Level Experience Needed
            /// </summary>
            public uint NextLevel { get; set; }

            /// <summary>
            /// Aislings Current HP Percentage
            /// </summary>
            public byte HPPercent { get { return (byte)(((decimal)CurHP / (decimal)MaxHP) * 100); } set { } }

            /// <summary>
            /// Aislings Current HP
            /// </summary>
            public uint CurHP { get; set; }

            /// <summary>
            /// Aislings Maximum HP
            /// </summary>
            public uint MaxHP { get; set; }

            /// <summary>
            /// Aislings Current MP
            /// </summary>
            public uint CurMP { get; set; }

            /// <summary>
            /// Aislings Maximum MP
            /// </summary>
            public uint MaxMP { get; set; }

            /// <summary>
            /// Aislings Current Gold Held
            /// </summary>
            public uint Gold { get; set; }

            /// <summary>
            /// Aislings Current GP
            /// </summary>
            public uint GP { get; set; }

            /// <summary>
            /// Aislings Current STR
            /// </summary>
            public byte STR { get; set; }

            /// <summary>
            /// Aislings Current INT
            /// </summary>
            public byte INT { get; set; }

            /// <summary>
            /// Aislings Current WIS
            /// </summary>
            public byte WIS { get; set; }

            /// <summary>
            /// Aislings Current CON
            /// </summary>
            public byte CON { get; set; }

            /// <summary>
            /// Aislings Current DEX
            /// </summary>
            public byte DEX { get; set; }

            /// <summary>
            /// Boolean As Byte : Aisling Has Stat Points Left?
            /// </summary>
            public byte HaveStats { get { return Convert.ToByte(Points > 0); } set { } }

            /// <summary>
            /// Aislings Current Stat Points Left
            /// </summary>
            public byte Points { get; set; }

            /// <summary>
            /// Aislings Maximum Cary Weight
            /// </summary>
            public ushort WeightMax { get; set; }

            /// <summary>
            /// Aislings Current Cary Weight
            /// </summary>
            public ushort WeightCurr { get; set; }

            /// <summary>
            /// Aislings Current AC
            /// </summary>
            public short AC { get; set; }

            /// <summary>
            /// Aislings Current MR
            /// </summary>
            public byte MR { get; set; }

            /// <summary>
            /// Aislings Current HIT
            /// </summary>
            public byte HIT { get; set; }

            /// <summary>
            /// Aislings Current DAM
            /// </summary>
            public byte DAM { get; set; }
            
            /// <summary>
            /// Aislings Attack Element
            /// </summary>
            public byte AttackElement { get; set; }

            /// <summary>
            /// Asilings Defense Element
            /// </summary>
            public byte DefenseElement { get; set; }
        }
        #endregion

        #region Appendage
        /// <summary>
        /// Aisling Sprite Body Object
        /// </summary>
        public class SpriteBody
        {
            /// <summary>
            /// Aisling Sprite Body Appendage Object
            /// </summary>
            public class Appendage
            {
                /// <summary>
                /// Default Appendage Constructor
                /// </summary>
                /// <param name="Icon">Appendage Icon</param>
                /// <param name="Name">Appendage Name</param>
                public Appendage(ushort Icon, string Name)
                {
                    Icon = (ushort)(Icon + 0x8000);
                    this.Name = Name;
                }

                /// <summary>
                /// Appendage Slot Enumeration
                /// </summary>
                public enum Slot { CannotWear = 0, Weapon = 1, Chest = 2, Shield = 3, Head = 4, Ear = 5, Neck = 6, LeftFinger = 7, RightFinger = 8, LeftArm = 9, RightArm = 10, Waist = 11, Legs = 12, Feet = 13, MidHead = 14 }

                /// <summary>
                /// Appendage Icon
                /// </summary>
                public ushort Icon;

                /// <summary>
                /// Appendage Name
                /// </summary>
                public string Name;
            }
            /// <summary>
            /// Aislings Appendage Array
            /// </summary>
            public Appendage[] Appendages = new Appendage[19];

            /// <summary>
            /// Aisling Appendage Acessor
            /// </summary>
            /// <param name="Slot">Appendage Slot</param>
            /// <returns>Appendage At Specified Slot</returns>
            public Appendage this[Appendage.Slot Slot]
            {
                get { return Appendages[(int)Slot]; }
                set { Appendages[(int)Slot] = value; }
            }
        }
        #endregion

        #region Sprite Animation
        /// <summary>
        /// Sprite Body Animation Object
        /// </summary>
        public class SpriteBodyAnimation
        {
            /// <summary>
            /// Default Sprite Body Animation Constructor
            /// </summary>
            /// <param name="Serial">Sprite Serial</param>
            /// <param name="Animation">Animation Number</param>
            /// <param name="Speed">Animation Speed</param>
            public SpriteBodyAnimation(uint Serial, byte Animation, byte Speed)
            {
                this.Serial = Serial;
                this.Animation = Animation;
                this.Speed = Speed;
                this.TimeStamp = DateTime.Now;
            }

            /// <summary>
            /// Sprite Serial
            /// </summary>
            public uint Serial;

            /// <summary>
            /// Sprite Animation
            /// </summary>
            public byte Animation;

            /// <summary>
            /// Sprite Speed
            /// </summary>
            public byte Speed;

            /// <summary>
            /// Date Time Of Animation
            /// </summary>
            public DateTime TimeStamp;
        }
        #endregion

        #region Bar Messages
        /// <summary>
        /// Bar Message Object
        /// </summary>
        public class BarMessage
        {
            /// <summary>
            /// Bar Message Constructor
            /// </summary>
            /// <param name="Type">Message Type</param>
            /// <param name="Message">Message</param>
            public BarMessage(MessageType Type, string Message)
            {
                this.Date = DateTime.Now;
                this.Type = Type;
                this.Message = Message;
            }

            /// <summary>
            /// Message Type Enumeration
            /// </summary>
            public enum MessageType
            {
                BlueWhisper = 0x00,
                OrangeBar = 0x03,
                WhiteEvent = 0x01,
                Dialog = 0x08,
                Sign = 0x0A,
                GreenWhisper = 0x0B,
                TealWhisper = 0x0C
            }

            /// <summary>
            /// Message Date Time
            /// </summary>
            public DateTime Date;

            /// <summary>
            /// Time Elapsed Since Message
            /// </summary>
            public TimeSpan TimeElapsed
            {
                get { return DateTime.Now - Date; }
            }

            /// <summary>
            /// Bar Message Type
            /// </summary>
            public MessageType Type;

            /// <summary>
            /// Message
            /// </summary>
            public string Message;
        }

        /// <summary>
        /// Clear Bar Messages
        /// </summary>
        public void BarMessages_Clear()
        {
            BarMessages.Clear();
        }
        #endregion

        #region Chat Messages
        /// <summary>
        /// Chat Message Object
        /// </summary>
        public class ChatMessage
        {
            /// <summary>
            /// Default Chat Message Constructor
            /// </summary>
            /// <param name="IsShout">Boolean: Is Message A Shout?</param>
            /// <param name="Serial">Entity Serial Number Who Said The Message</param>
            /// <param name="Message">Message</param>
            public ChatMessage(bool IsShout, uint Serial, string Message)
            {
                this.IsShout = IsShout;
                this.Serial = Serial;
                this.Message = Message;
            }

            /// <summary>
            /// Boolean: Is Message A Shout?
            /// </summary>
            public bool IsShout;

            /// <summary>
            /// Entity Serial Number Who Said The Message
            /// </summary>
            public uint Serial;

            /// <summary>
            /// Message
            /// </summary>
            public string Message;
        }

        /// <summary>
        /// Clear Chat Messages
        /// </summary>
        public void ChatMessages_Clear()
        {
            ChatMessages.Clear();
        }
        #endregion

        public string Password;
        public string Username;

        public DateTime TargetedTime = DateTime.Now;
    }
}