using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fish : Item, IFood , IThermal
{
    public Item_Fish(int itemID) : base(itemID, "물고기", "신선한 물고기 \n허기를 2 채워준다.", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/303"))
    {
        durability = 1;
    }

    public int Fullness => 2;

    public int MaxDurability => 1;

    protected int durability;
    public int Durability => durability;

    public float Thermal => 4;

    public void Consume()
    {
        durability -= 1;
        if (durability <= 0)
        {
            Debug.Log("아이템 파괴");
        }
    }

    public Item Done()
    {
        //자신을 구운 물고기로 바꿔야 하는데
        return GameManager.Instance.GetItem(13);
    }
}
