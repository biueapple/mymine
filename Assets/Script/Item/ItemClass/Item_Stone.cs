using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Stone : Item, IInstall
{
    public Item_Stone(int itemID) : base (itemID, "��", "��ġ�� ������ ��", Item_Rating.COMMON, true, Resources.Load<Sprite>("Icon/Sword"))
    {
        blockID = 1;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
