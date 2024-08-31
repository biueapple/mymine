using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipItemSlot : ItemSlot
{
    [SerializeField]
    protected Equipment_Part[] parts;
    public Equipment_Part[] Parts { get { return parts; } }

    private Action<EquipItemSlot> beforeUpdate;
    public Action<EquipItemSlot> BeforeEquip { get { return beforeUpdate; } set { beforeUpdate = value; } }
    private Action<EquipItemSlot> afterUpdate;
    public Action<EquipItemSlot> AfterEquip { get { return afterUpdate; } set { afterUpdate = value; } }

    public EquipItemSlot(Equipment_Part[] parts, bool input, bool output) : base(input, output)
    {
        this.parts = parts;
    }
    public EquipItemSlot(Equipment_Part part, bool input, bool output) : base(input, output)
    {
        parts = new Equipment_Part[1] { part };
    }
    public EquipItemSlot(bool input, bool output) : base(input, output)
    {
        parts = null;
    }

    public override void Update(Item item, int amount)
    {
        beforeUpdate?.Invoke(this);
        base.Update(item, amount);
        afterUpdate?.Invoke(this);
    }

    public override bool Necessary(ItemSculpture itemSculpture)
    {
        if (input)
        {
            if (this.itemSculpture.Item == null || this.itemSculpture.Amount == 0)
            {
                if (itemSculpture.Item is IEquipment equipment)
                {
                    if (parts == null || parts.Length == 0)
                    {
                        return true;
                    }
                    else
                    {
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i] == equipment.EquipmentPart)
                            {
                                return true;
                            }
                        }
                    }
                } 
            }
        }
        return false;
    }
}
