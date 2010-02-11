using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Spell Cast Event Delegate
    /// </summary>
    /// <param name="Spell">Spell Casted</param>
    public delegate void OnSpellCast(Spell Spell);

    /// <summary>
    /// Spell Object
    /// </summary>
    public class Spell
    {
        /// <summary>
        /// Spell Slot
        /// </summary>
        public byte Slot { get; set; }

        /// <summary>
        /// Serial Number
        /// </summary>
        public uint Serial { get; set; }
    }

    #region Enums
    /// <summary>
    /// Spell Icon Enumeration
    /// </summary>
    public enum spellIcon
    {
        Disease = 0x01,
        Mezmerize__WormSkull__Shout = 0x02,
        Blind = 0x03,
        beag_cradh = 0x05,
        eisd_creutair = 0x07,
        Hide = 0x0A,
        naomh_aite = 0x0B,
        creag_neart = 0x0D,
        beannaich = 0x10,
        Perfect_Defense__Weather_Protection = 0x13,
        slan__Silence = 0x1A,
        Poison = 0x23,
        Burn__Pause = 0x28,
        Sleep = 0x32,
        fas_deireas = 0x34,
        dion = 0x35,
        deireas_faileas__asgall_faileas = 0x36,
        Mist = 0x37,
        cradh = 0x52,
        mor_cradh = 0x53,
        ard_cradh = 0x54,
        Skull = 0x59,
        pramh = 0x5A,
        armachd__Aegis_Sphere = 0x5E,
        beag_suain = 0x61,
        Claw_Fist = 0x63,
        Wolf_Fang_Fist = 0x65,
        fas_nadur = 0x77,
        Cats_Hearing = 0x7C,
        Inner_Fire = 0x7E,
        Dark_Seal = 0x85,
        Decay = 0x8D,
        Purify = 0x8F,
        Regeneration = 0x92,
        Counter_Attack = 0x96,
        Feral_Form = 0xB7,
        Lizard_Form = 0xB9,
        Bird_Form = 0xB8
    }
    #endregion
}
