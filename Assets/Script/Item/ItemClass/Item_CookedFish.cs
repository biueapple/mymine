using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_CookedFish : Item, IFood
{
    public Item_CookedFish(int itemID) : base(itemID, "���� ����", "���� ����",  Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/304"))
    {
        durability = 1;
    }

    public int Fullness => 4;

    public int MaxDurability => 1;

    private int durability; 
    public int Durability => durability;

    public void Consume()
    {
        durability -= 1;
        if (durability <= 0)
        {
            Debug.Log("������ �ı�");
        }
    }
}
