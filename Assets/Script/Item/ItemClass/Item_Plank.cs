using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Plank : Item, IInstall
{
    public Item_Plank(int itemID) : base(itemID, "나무막대기", "나무로 만들어진 막대기", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 9;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
