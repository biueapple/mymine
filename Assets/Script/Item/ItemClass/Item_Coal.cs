using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Coal : Item
{
    public Item_Coal(int itemID) : base(itemID, "��ź", "��ź", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {

    }
}
