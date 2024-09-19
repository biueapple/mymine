using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_IronOre : Item, IThermal
{
    public Item_IronOre(int itemID) : base(itemID, "ö ����", "����� ö �ֱ��� �Ǵ� ö ����", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/15"))
    {

    }

    public float Thermal => 8;

    public Item Done()
    {
        return GameManager.Instance.GetItem(15);
    }
}
