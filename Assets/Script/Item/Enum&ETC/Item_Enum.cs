

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
    NONE,   //�ƹ��͵� �ƴ�
    PICKAX, //���
    AXE,    //����
    SHOVELS,//��
    HOE,    //����

}

public enum DamageType
{
    AD,
    AP,
    TRUE,
}

//���⸦ ��� ��ų�� ������ ��ȭ�ؼ� �⺻������ �� �� ȿ���� �߰� ������ ���ٸ�
public enum AttackType
{
    NONE,       //�׳� �߰�����
    NOMAL,      //�⺻ ����
    WEAPON,     //����� ���� ����
    SPECIAL     //��ų�����ɷ� ���� ����
}