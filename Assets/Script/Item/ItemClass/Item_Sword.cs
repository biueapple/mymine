using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item_Sword : Item, IWeapon, IConsume, IRepairItem
{
    public Item_Sword(int itemID, Item_Rating rating) : base(itemID, "평범한 칼", "평범한 칼이다", rating, false, Resources.Load<Sprite>("Item/Sword"))
    {
        attributePossibilities = new AttributePossibility[4];
        attributePossibilities[0] = new AttributePossibility(Attribute_Property.HP, 2, 4);
        attributePossibilities[1] = new AttributePossibility(Attribute_Property.AD, 2, 4);
        attributePossibilities[2] = new AttributePossibility(Attribute_Property.DEFENCEPENETRATION, 2, 4);
        attributePossibilities[3] = new AttributePossibility(Attribute_Property.ATTACKSPEED, 1, 2);

        attributePieces = AttributeDecision();

        equipmentPart = Equipment_Part.WEAPON;

        maxDurability = 100;
        durability = 100;

        motionCount = 2;
    }

    public Item_Sword(int itemID) : base(itemID, "평범한 칼", "평범한 칼이다", (Item_Rating)Random.Range(0, (int)Item_Rating.EPIC + 1) , false, Resources.Load<Sprite>("Item/Sword"))
    {
        attributePossibilities = new AttributePossibility[4];
        attributePossibilities[0] = new AttributePossibility(Attribute_Property.HP, 2, 4);
        attributePossibilities[1] = new AttributePossibility(Attribute_Property.AD, 2, 4);
        attributePossibilities[2] = new AttributePossibility(Attribute_Property.DEFENCEPENETRATION, 2, 4);
        attributePossibilities[3] = new AttributePossibility(Attribute_Property.ATTACKSPEED, 1, 2);

        attributePieces = AttributeDecision();

        equipmentPart = Equipment_Part.WEAPON;

        maxDurability = 100;
        durability = 100;

        motionCount = 2;
    }

    [SerializeField]
    protected AttributePossibility[] attributePossibilities;
    public AttributePossibility[] AttributePossibilities => attributePossibilities;

    [SerializeField]
    protected AttributePiece[] attributePieces;
    public AttributePiece[] AttributePieces => attributePieces;

    [SerializeField]
    protected Equipment_Part equipmentPart;
    public Equipment_Part EquipmentPart => equipmentPart;

    [SerializeField]
    protected int maxDurability;
    public int MaxDurability => maxDurability;

    [SerializeField]
    protected int durability;
    public int Durability => durability;

    [SerializeField]
    protected int motionCount;
    public int MotionCount => motionCount;

    public int RepairItem(int figure)
    {
        figure = Mathf.Min(maxDurability - durability, figure);
        durability += figure;
        return figure;
    }

    protected AttributePiece[] AttributeDecision()
    {
        AttributePiece[] attributePieces = rating switch
        {
            Item_Rating.COMMON => new AttributePiece[2],
            Item_Rating.UNCOMMON => new AttributePiece[3],
            Item_Rating.RARE => new AttributePiece[4],
            Item_Rating.EPIC => new AttributePiece[5],
            Item_Rating.UNIQUE => new AttributePiece[2],
            Item_Rating.LEGEND => new AttributePiece[3],
            _ => new AttributePiece[0]
        };

        int index;
        // 배열을 채웁니다.
        for (int i = 0; i < attributePieces.Length; i++)
        {
            index = Random.Range(0, attributePossibilities.Length);
            attributePieces[i] = new AttributePiece( attributePossibilities[index].Property, attributePossibilities[index].Value());
        }

        return attributePieces;
    }

    public void Consume()
    {
        durability -= 1;
        if (durability <= 0)
        {
            Debug.Log("아이템 파괴");
        }
    }
}
