using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Map Entity Object (Marshall By Reference Object)
    /// </summary>
    public class MapEntity : MarshalByRefObject
    {
        /// <summary>
        /// Default Map Entity Constructor
        /// </summary>
        /// <param name="EntityType">Type Of Entity</param>
        public MapEntity(Type EntityType)
        {
            this.HPPercent = 100;
            this.EntityType = EntityType;
            PossibleDeathTime = new DateTime(0);
            this.WasHit = false;
            this.CanAttack = false;
        }

        /// <summary>
        /// Map Entity Type Enumeration
        /// </summary>
        public enum Type
        {
            Player,
            Item,
            Monster,
            NPC
        }

        public byte HPPercent { get; set; }

        /// <summary>
        /// Type Of Map Entity
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// Entity Icon
        /// </summary>
        public ushort Icon { get; set; }

        /// <summary>
        /// Entity Serial Number
        /// </summary>
        public uint Serial { get; set; }

        /// <summary>
        /// Entity Location
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Boolean: Is Entity Banned?
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// Date Time Of Possible Death Of Entity
        /// </summary>
        public DateTime PossibleDeathTime { get; set; }

        /// <summary>
        /// Boolean: Ignore Possible Death Of Entity?
        /// </summary>
        public bool IgnoreDeathTime { get; set; }

        /// <summary>
        /// Boolean: Was Entity Hit?
        /// </summary>
        public bool WasHit { get; set; }

        /// <summary>
        /// Boolean: Can Attack Entity?
        /// </summary>
        public bool CanAttack { get; set; }

        #region Curses
        /// <summary>
        /// Boolean: Has Dark Seal?
        /// </summary>
        public bool HasDarkSeal { get; set; }
        /// <summary>
        /// Boolean: Has Ard Cradh
        /// </summary>
        public bool HasArdCradh { get; set; }
        /// <summary>
        /// Boolean: Has Mor Cradh
        /// </summary>
        public bool HasMorCradh { get; set; }
        /// <summary>
        /// Boolean: Has Cradh
        /// </summary>
        public bool HasCradh { get; set; }
        /// <summary>
        /// Boolean: Has Beag Cradh
        /// </summary>
        public bool HasBeagCradh { get; set; } 
        /// <summary>
        /// Boolean: Was Fassed
        /// </summary>
        public bool WasFased { get; set; }
        /// <summary>
        /// Boolean: Was Suained
        /// </summary>
        public bool WasSuained { get; set; }
        /// <summary>
        /// Boolean: Was Pramhed?
        /// </summary>
        public bool WasPramhed { get; set; }
        /// <summary>
        /// Number Of Suain Animations
        /// </summary>
        public int SuainCount { get; set; }
        #endregion

        public List<Animation> Animations = new List<Animation>();
    }
}
