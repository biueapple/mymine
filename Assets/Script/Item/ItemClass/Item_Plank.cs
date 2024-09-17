using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Plank : Item, IInstall , IFlammable
{
    public Item_Plank(int itemID) : base(itemID, "���������", "������ ������� �����", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/5"))
    {
        
    }

    public int BlockID => 9;

    public float Flammable => 3;
}
