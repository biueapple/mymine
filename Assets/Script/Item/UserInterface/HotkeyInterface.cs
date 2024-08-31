using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotkeyInterface : DynamicInterface
{
    protected int select;
    public int Select { get { return select; } }

    public ItemSlot SelectSlot { get { return storage.Slots[select]; } }

    private Transform highlight;

    public void Choice(int index)
    {
        select = index;

        if (select < 0)
            select = storage.Slots.Length - 1;
        else if (select >= storage.Slots.Length)
            select = 0;

        highlight.transform.localPosition = new Vector3(X_START + X_SPACE_BETWWEN_ITEM * select, 0, 0);
    }

    public void Change(int index)
    {
        select += index;

        if (select < 0)
            select = storage.Slots.Length - 1;
        else if (select >= storage.Slots.Length)
            select = 0;

        highlight.transform.localPosition = new Vector3(X_START + X_SPACE_BETWWEN_ITEM * select, 0, 0);
    }

    protected override void CreateSlot()
    {
        highlight = transform.GetChild(0);
        base.CreateSlot();
    }
}

