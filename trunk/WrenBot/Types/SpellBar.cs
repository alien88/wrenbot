using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Spell Bar Event Delegate
    /// </summary>
    /// <param name="SpellBar">Spell Bar</param>
    public delegate void SpellBarHandler(SpellBar SpellBar);

    /// <summary>
    /// Spell Bar Object
    /// </summary>
    public class SpellBar
    {
        /// <summary>
        /// Default Spell Bar Constructor
        /// </summary>
        /// <param name="Icon">Icon</param>
        /// <param name="Color">Color</param>
        public SpellBar(ushort Icon, WrenBot.Net.ServerStructs.PlayerSpellBar.SpellIconColor Color)
        {
            this.Icon = Icon;
            this.Color = Color;
        }

        /// <summary>
        /// Icon Number
        /// </summary>
        public ushort Icon;

        /// <summary>
        /// Spell Icon Color
        /// </summary>
        public WrenBot.Net.ServerStructs.PlayerSpellBar.SpellIconColor Color;

        /// <summary>
        /// Spell Bar Event Handler
        /// </summary>
        public SpellBarHandler OnAffliction;
    }
}
