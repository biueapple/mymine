using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Wood : Item, IInstall , IFlammable
{
    public Item_Wood(int itemID) : base(itemID, "����", "������ ��պκ�", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/17"))
    {

    }

    public int BlockID => 7;

    public float Flammable => 5;
}
