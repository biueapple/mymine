using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_WoodenStick : Item
{
    public Item_WoodenStick(int itemID) : base(itemID, "���������", "������ ������� �����", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
    }
}
