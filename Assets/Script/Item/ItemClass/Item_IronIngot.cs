using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_IronIngot : Item
{
    public Item_IronIngot(int itemID) : base(itemID, "ö �ֱ�", "�ܴ��� ö �ֱ�", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/263"))
    {

    }
}
