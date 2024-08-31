using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : Block , IBlockOpenUI
{
    private static WorkbenchInterface workbenchInterface;
    public static WorkbenchInterface WorkbenchInterface
    {
        get
        {
            if(workbenchInterface == null)
            {
                workbenchInterface = Object.Instantiate(Resources.Load<WorkbenchInterface>("UI/UserInterface/WorkbenchUI"));
                workbenchInterface.transform.SetParent(UIManager.Instance.Canvas.transform);
                workbenchInterface.transform.localPosition = new Vector3(0, 0, 0);
            }
            return workbenchInterface;
        }
    }
    private readonly Storage storage;

    public Workbench(int id) : base(id, "조합대", HardnessType.AXE, 2, false, true, 6, new int[6] { 195, 196, 197, 144, 196, 196 })
    {
        ItemSlot[] itemSlots = new ItemSlot[10];
        for (int i = 0; i < 9; i++)
        {
            itemSlots[i] = new ItemSlot(true, true);
            itemSlots[i].AfterUpdate += CraftingCheck;
        }
        itemSlots[9] = new ItemSlot(false, true);
        itemSlots[9].BeforeUpdate += ResultOutput;

        storage = new Storage(itemSlots);
    }

    public void OpenUI(Player player, World.BlockLaycast blockLaycast)
    {
        WorkbenchInterface.Interlock(storage);
        UIManager.Instance.OpenUI(WorkbenchInterface.gameObject);
    }

    private void CraftingCheck(ItemSlot slot)
    {
        Table matrix = new Table3X3();
        int[,] ints = new int[3, 3]
        {
            { storage.Slots[0].Item != null ? storage.Slots[0].Item.ItemID : 0, storage.Slots[1].Item != null ? storage.Slots[1].Item.ItemID : 0, storage.Slots[2].Item != null ? storage.Slots[2].Item.ItemID : 0},
            { storage.Slots[3].Item != null ? storage.Slots[3].Item.ItemID : 0, storage.Slots[4].Item != null ? storage.Slots[4].Item.ItemID : 0, storage.Slots[5].Item != null ? storage.Slots[5].Item.ItemID : 0},
            { storage.Slots[6].Item != null ? storage.Slots[6].Item.ItemID : 0, storage.Slots[7].Item != null ? storage.Slots[7].Item.ItemID : 0, storage.Slots[8].Item != null ? storage.Slots[8].Item.ItemID : 0}
        };
        matrix.codes = ints;
        matrix.Slice(matrix, out matrix);
        int id;
        int count;
        Crafting.Instance.Combination(matrix, out id, out count);

        Item item = GameManager.Instance.GetItem(id);
        //조합에 맞는 아이템이 이었던 경우
        if (item != null)
        {
            storage.Slots[9].BeforeUpdate -= ResultOutput;
            storage.Slots[9].Update(item, count);
            storage.Slots[9].BeforeUpdate += ResultOutput;
        }
        //조합에 맞는 아이템이 없었던 경우
        else if (item == null && storage.Slots[9].Item != null)
        {
            storage.Slots[9].BeforeUpdate -= ResultOutput;
            storage.Slots[9].Update(null, 0);
            storage.Slots[9].BeforeUpdate += ResultOutput;
        }
    }

    public void ResultOutput(ItemSlot slot)
    {
        for (int i = 0; i < 9; i++)
        {
            if (storage.Slots[i].Item != null)
                storage.Slots[i].Update(storage.Slots[i].Item, storage.Slots[i].Amount - 1);
        }
    }
}
