using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StaticInterface : StorageInterface
{
    [SerializeField]
    protected ItemSlotUI[] slots;
    public ItemSlotUI[] Slots { get { return slots; } }

    protected override void CreateSlot()
    {
        if (storage != null)
        {
            for (int i = 0; i < storage.Slots.Length; i++)
            {
                slots[i].Init(storage.Slots[i]);
            }
        }
    }
}


