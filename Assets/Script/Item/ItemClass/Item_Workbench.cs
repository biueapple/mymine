using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Workbench : Item, IInstall , IFlammable
{
    public Item_Workbench(int itemID) : base(itemID, "���մ�", "�������� ������ �� �ְ� ���ִ� ���մ�", Item_Rating.COMMON, true, Resources.Load<Sprite>("Item/66"))
    {
        
    }

    public int BlockID => 4;

    public float Flammable => 4;
}
