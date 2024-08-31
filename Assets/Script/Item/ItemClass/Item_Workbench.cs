using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Workbench : Item, IInstall
{
    public Item_Workbench(int itemID) : base(itemID, "조합대", "아이템을 조합할 수 있게 해주는 조합대", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 4;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
