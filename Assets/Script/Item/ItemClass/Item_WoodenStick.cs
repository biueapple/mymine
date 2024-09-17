using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WoodenStick : Item , IFlammable
{
    public Item_WoodenStick(int itemID) : base(itemID, "나무막대기", "나무로 만들어진 막대기", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/262"))
    {
    }

    public float Flammable => 1;
}
