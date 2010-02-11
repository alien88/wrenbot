using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrenBot.Types
{
    /// <summary>
    /// Profile Object (Used For Saving Form Settings On Exit)
    /// </summary>
    [Serializable]
    public class Profile
    {
        #region Basher
        // Filtering
        public bool b_EnableLooting = false;
        public List<ushort> b_Loots = new List<ushort>();
        public List<ushort> b_BlackList = new List<ushort>();
        public string[] b_comboset = new string[0];
        public string[] b_skillset = new string[0];

        // Settings
        public bool b_EnableFCLogic = false;
        public bool b_WalkToBack = false;
        public bool b_AllowDion = false;
        public bool b_StayDioned = false;
        public bool b_UseKelb = false;
        public bool b_NeverLeaveCaster = false;
        public int b_AreaOfView = 15;
        public int b_DisengageFactor = 5;
        public int b_MobSize = 1;
        public bool b_WaitForSpells = false; 
        #endregion

        #region Caster
        // Group
        public int c_FollowDistance = 3;
        public int c_NarrowDistance = 20;

        // Spell Config - Defensive
        public bool c_FasParty = false;
        public bool c_AllowDion = false;
        public bool c_DisableSync = false;
        public bool c_MdcBasher = false;
        public bool c_MorBeann = false;
        public bool c_Regen = false;
        public bool c_CounterAttack = false;
        public bool c_FasDerias = false;
        public bool c_Armachd = false;
        public bool c_DerieasFeileas = false;

        // Spell Config - Offensive
        public bool c_Suain = false;
        public bool c_FasTargets = false;
        public bool c_Prahm = false;
        public bool c_SpellAll = false;

        //excess
        public bool c_excess = false;

        // Spell Config - Logic
        public int c_SpellAllDistance = 3;
        public int c_SpellDistance = 3;

        // Spell Config - After Spelling
        public bool c_DeoSearg = false;
        public int c_DeoSeargMP = 20000;
        public bool c_ElementAttack = false;
        public int c_ElementAttackMP = 15000;
        public bool c_curse = false;

        // General
        public string SpellChant = "1";
        public int c_CastDelay = 200;
        public int c_SpellCheck = 15;
        public bool c_DisableZeroLine = false;
        public bool c_LootGold = false;
        public int c_cd = 8;
        #endregion

        // Options
        public int WalkSpeed = 385;
        public bool SlowWhenAround = false;
        public bool IgnoreGroupRequest = false;
        public bool SeeInvis = false;
        public bool AutoAcceptGroup = false;
        public bool ReportMonsterImage = false;
        public bool SwapOnBreak = false;

        public List<string> b_invfilter = new List<string>();

        //Alerts
        public string ChatSound = "";
        public string WhisperSound = "";
        public bool Enable_Alerts = false;

        // Security
        public List<string> BanList = new List<string>();

        // Misc
        public List<string> AttackSpells = new List<string>();
    }
}
