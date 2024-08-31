using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Workbench : Item, IInstall
{
    public Item_Workbench(int itemID) : base(itemID, "���մ�", "�������� ������ �� �ְ� ���ִ� ���մ�", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/Sword"))
    {
        blockID = 4;
    }

    protected readonly int blockID;
    public int BlockID => blockID;
}
