using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Furnace : Item, IInstall
{
    public Item_Furnace(int itemID) : base(itemID, "ȭ��", "ȭ��", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/68"))
    {
    }

    public int BlockID => 11;
}
