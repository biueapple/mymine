using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fish : Item, IFood , IThermal
{
    public Item_Fish(int itemID) : base(itemID, "�����", "�ż��� ����� \n��⸦ 2 ä���ش�.", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/303"))
    {
        fullness = 2;
        maxDurability = 1;
        durability = 1;
    }

    protected int fullness;
    public int Fullness => fullness;

    protected int maxDurability;
    public int MaxDurability => maxDurability;

    protected int durability;
    public int Durability => durability;

    public float Thermal => 4;

    public void Consume()
    {
        durability -= 1;
        if (durability <= 0)
        {
            Debug.Log("������ �ı�");
        }
    }

    public Item Done()
    {
        //�ڽ��� ���� ������ �ٲ�� �ϴµ�
        return this;
    }
}
