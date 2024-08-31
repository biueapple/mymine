using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Wood : Item, IInstall
{
    public Item_Wood(int itemID) : base(itemID, "����", "������ ��պκ�", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 7;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
