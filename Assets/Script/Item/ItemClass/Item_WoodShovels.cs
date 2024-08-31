using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WoodShovels : Item, Strength_Shovels , IConsume
{
    public Item_WoodShovels(int itemID) : base(itemID, "���� ��", "��Ŭ������ ���� �������� �ٲ� �� �ִ�.", Item_Rating.COMMON, false, Resources.Load<Sprite>("Item/Sword"))
    {
        strength = 2;
        maxDurability = 100;
        durability = 100;
    }

    protected int strength;
    public int Strength => strength;

    protected int maxDurability;
    public int MaxDurability => maxDurability;

    protected int durability;
    public int Durability => durability;

    public void Consume()
    {
        durability -= 1;
        if (durability <= 0)
        {
            Debug.Log("������ �ı�");
        }
    }
}
