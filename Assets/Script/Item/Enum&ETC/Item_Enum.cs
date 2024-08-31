

public enum Equipment_Part
{
    NONE,
    HELMET,
    TOP,
    BOTTOM,
    BOOTS,
    WEAPON,
    SHIELD,
    RUNE,
}

public enum Item_Rating
{
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    UNIQUE,
    LEGEND
}

public enum Attribute_Property
{
    HP,
    MP,
    BARRIER,
    NATURALHP,
    NATURALMP,
    NATURALARRIER,
    DEFENCE,
    RESISTANCE,
    AD,
    AP,
    ATTACKSPEED,
    CRITICALCHANCE,
    CRITICALMULTIPLIER,
    CRITICALDEFENCE,
    DEFENCEPENETRATION,
    DEFENCEPENETRATIONPER,
    RESISTANCEPENETRATION,
    RESISTANCEPENETRATIONPER,
    SPEED,
}


public enum HardnessType
{
    NONE,   //아무것도 아님
    PICKAX, //곡괭이
    AXE,    //도끼
    SHOVELS,//삽
    HOE,    //괭이

}

public enum DamageType
{
    AD,
    AP,
    TRUE,
}

//무기를 들고 스킬로 공격을 강화해서 기본공격을 한 후 효과로 추가 공격이 들어간다면
public enum AttackType
{
    NONE,       //그냥 추가공격
    NOMAL,      //기본 공격
    WEAPON,     //무기로 인한 공격
    SPECIAL     //스킬같은걸로 인한 공격
}