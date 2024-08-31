using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSculpture
{
    [SerializeField]
    private Item item;
    public Item Item { get { return item; } }
    [SerializeField]
    private int amount;
    public int Amount { get { return amount; } }

    public ItemSculpture(Item item, int amount)
    {
        if (amount <= 0)
        {
            this.item = null;
            this.amount = 0;
            return;
        }
        this.item = item;
        this.amount = amount;
    }

    public void Input(ItemSculpture itemSculpture)
    {
        item = itemSculpture.item;
        amount = itemSculpture.amount; 
        if (amount <= 0)
        {
            item = null;
            amount = 0;
        }
    }
    public void Input(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
        if (amount <= 0)
        {
            this.item = null;
            this.amount = 0;
        }
    }
    public void Output(ItemSculpture itemSculpture)
    {
        itemSculpture.item = item;
        itemSculpture.amount = amount;
    }
}
