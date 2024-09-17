using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bone : Item
{
    public Item_Bone(int itemID) : base(itemID, "»À", "±×³É »À", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/345"))
    {

    }
}