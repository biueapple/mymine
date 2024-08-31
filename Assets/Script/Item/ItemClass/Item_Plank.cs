using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Plank : Item, IInstall
{
    public Item_Plank(int itemID) : base(itemID, "���������", "������ ������� �����", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 9;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
