using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WrenBot.Net.ServerStructs;

namespace WrenBot.Types
{
    /// <summary>
    /// Aisling Entity Object (Inherits Map Entity)
    /// </summary>
    public class AislingEntity : MapEntity
    {
        /// <summary>
        /// Default Aisling Entity Constructor
        /// </summary>
        public AislingEntity() : base(Type.Player)
        {
            Title = "";
            GuildName = "";
            GuildRank = "";
            Equipment = new ushort[18];
            LegendInfo = new LegendInfo.Entry[0];
            Icon = 0;
            MessageLog = new List<string>();
        }

        public List<uint> Targets = new List<uint>();

        /// <summary>
        /// Aisling Entity Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Aisling Entity Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Aisling Entity Guild Name
        /// </summary>
        public string GuildName { get; set; }

        /// <summary>
        /// Aisling Entity Guild Rank
        /// </summary>
        public string GuildRank { get; set; }

        /// <summary>
        /// Aisling Entity Hover Message
        /// </summary>
        public string HoverMessage { get; set; }

        /// <summary>
        /// Aisling Entity Equipment Array
        /// </summary>
        public ushort[] Equipment { get; set; }

        /// <summary>
        /// Aisling Entity Legend Entry Array
        /// </summary>
        public LegendInfo.Entry[] LegendInfo { get; set; }

        /// <summary>
        /// Aisling Entitys Last Assail Location
        /// </summary>
        public Location LastAssailLocation { get; set; }

        #region ChatBot
        /// <summary>
        /// Message Log List
        /// </summary>
        public List<string> MessageLog;
        #endregion
    }
}
