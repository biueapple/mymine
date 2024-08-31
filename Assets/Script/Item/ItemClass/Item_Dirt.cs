using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Dirt : Item, IInstall
{
    public Item_Dirt(int itemID) : base(itemID, "Èë", "±×³É Èë", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 2;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
