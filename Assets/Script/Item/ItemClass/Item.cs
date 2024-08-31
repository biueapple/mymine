using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : IItem
{
    [SerializeField]
    protected readonly int itemID;
    public int ItemID { get { return itemID; } }

    [SerializeField]
    protected readonly string name;
    public string Name { get { return name; } }

    [SerializeField]
    protected readonly string description;
    public string Description { get { return description; } }

    [SerializeField]
    protected readonly Item_Rating rating;
    public Item_Rating Item_Rating { get { return rating; } }

    [SerializeField]
    protected readonly bool stackable;
    public bool Stackable { get { return stackable; } }

    [SerializeField]
    protected readonly Sprite icon;
    public Sprite Icon { get { return icon; } }

    public Item(int itemID, string name, string description, Item_Rating rating, bool stackable, Sprite icon)
    {
        this.itemID = itemID;
        this.name = name;
        this.description = description;
        this.rating = rating;
        this.stackable = stackable;
        this.icon = icon;
    }

    public static bool operator==(Item a, Item b)
    {
        if (Object.ReferenceEquals(a, null) || Object.ReferenceEquals(b, null))
            return Object.ReferenceEquals(a, b); // 두 개가 모두 null이면 true, 하나만 null이면 false

        return a.ItemID == b.ItemID;
    }
    public static bool operator!=(Item a, Item b)
    {
        return !(a == b);
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Item))
            return false;

        Item item = (Item)obj;
        return itemID == item.ItemID;
    }
    public override int GetHashCode()
    {
        return itemID.GetHashCode();
    }
}
