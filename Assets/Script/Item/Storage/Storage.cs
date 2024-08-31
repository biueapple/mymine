using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Storage
{
    [SerializeField]
    private ItemSlot[] slots;
    public ItemSlot[] Slots { get { return slots; } }

    public Storage(int length, bool input, bool output)
    {
        slots = new ItemSlot[length];
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot(input, output);
        }
    }

    public Storage(ItemSlot[] itemSlots) 
    {
        slots = itemSlots;
    }


    public int Acquire(Item item, int amount)
    {
        if(item.Stackable)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Item == item)
                {
                    if(slots[i].Take(new ItemSculpture(item, amount)))
                        return 0;
                }
            }

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Item == null)
                {
                    if(slots[i].Take(new ItemSculpture(item, amount)))
                        return 0;
                }
            }
        }
        else
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].Item == null)
                {
                    if(slots[i].Take(new ItemSculpture(item, 1)))
                    {
                        amount--;
                        if (amount <= 0)
                            return amount;
                    }
                }
            }
        }
        
        return amount;
    }
}
