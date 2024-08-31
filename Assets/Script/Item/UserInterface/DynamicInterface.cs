using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInterface : StorageInterface
{
    protected ItemSlotUI slot;
    //ui의 시작지점
    [SerializeField]
    protected int X_START;
    [SerializeField]
    protected int Y_START;
    //ui간의 얼마만큼 떨어져 있는지 한줄에 몇개가 들어가는지
    [SerializeField]
    protected int X_SPACE_BETWWEN_ITEM;
    [SerializeField]
    protected int NUMBER_OF_COLUMN;
    [SerializeField]
    protected int Y_SPACE_BETWWEN_ITEMS;

    protected override void CreateSlot()
    {
        if (slot == null)
            slot = Resources.Load<ItemSlotUI>("UI/Slot/SlotUI");
        if (storage != null)
        {
            ItemSlotUI slotUI;
            for (int i = 0; i < storage.Slots.Length; i++)
            {
                //만들고
                slotUI = Instantiate(slot, transform);
                //위치정하고
                slotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);
                //초기화
                slotUI.Init(storage.Slots[i]);
            }
        }
    }

    //ui포지션을 계산해줌
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWWEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWWEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0);
    }
}

