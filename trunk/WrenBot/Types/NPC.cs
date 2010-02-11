using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// NPC Object (Inherits MapEntity)
    /// </summary>
    public class NPC : MapEntity
    {
        /// <summary>
        /// Default NPC Constructor
        /// </summary>
        public NPC() : base(Type.NPC) { }

        /// <summary>
        /// NPC Name
        /// </summary>
        public string Name { get; set; }
    }
}