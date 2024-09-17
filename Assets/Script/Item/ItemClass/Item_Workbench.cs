using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Workbench : Item, IInstall , IFlammable
{
    public Item_Workbench(int itemID) : base(itemID, "조합대", "아이템을 조합할 수 있게 해주는 조합대", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/66"))
    {
        
    }

    public int BlockID => 4;

    public float Flammable => 4;
}
