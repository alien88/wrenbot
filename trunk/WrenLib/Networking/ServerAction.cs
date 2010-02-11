
namespace WrenLib
{
    public enum ServerAction
    {
        Redirect = 0x03,
        Location = 0x04,
        Serial = 0x05,
        AddSprites = 0x07,
        StatsUpdated = 0x08,
        Bar = 0x0A,
        ClientWalk = 0x0B,
        EntityWalked = 0x0C,
        Chat = 0x0D,
        RemoveSprite = 0x0E,
        ItemSlotInfo = 0x0F,
        RemoveItem = 0x10,
        EntityTurn = 0x11,
        DisplayHPBAR = 0x13,
        MapInfo = 0x15,
        SpellSlotInfo = 0x17,
        RemoveSpell = 0x18,
        SoundPlay = 0x19,
        BodyAnimation = 0x1A,
        Animation = 0x29,
        SkillSlotInfo = 0x2C,
        RemoveSkill = 0x2D,
        Wall = 0x32,
        AddPlayer = 0x33,
        LegendInfo = 0x34,
        Appendage = 0x37,
        RemoveAppendage = 0x38,
        SpellBar = 0x3A,
        GroupRequest = 0x63,
        Ping = 0x3B,
        Cooldown = 0x3F,
        Tick = 0x68,
        PopUpResponse = 0x30,
        CountryList = 0x36
    }
}
