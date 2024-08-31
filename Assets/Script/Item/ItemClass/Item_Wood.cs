using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Wood : Item, IInstall
{
    public Item_Wood(int itemID) : base(itemID, "나무", "나무의 기둥부분", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 7;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
