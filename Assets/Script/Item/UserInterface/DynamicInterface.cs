using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicInterface : StorageInterface
{
    protected ItemSlotUI slot;
    //ui�� ��������
    [SerializeField]
    protected int X_START;
    [SerializeField]
    protected int Y_START;
    //ui���� �󸶸�ŭ ������ �ִ��� ���ٿ� ��� ������
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
                //�����
                slotUI = Instantiate(slot, transform);
                //��ġ���ϰ�
                slotUI.GetComponent<RectTransform>().localPosition = GetPosition(i);
                //�ʱ�ȭ
                slotUI.Init(storage.Slots[i]);
            }
        }
    }

    //ui�������� �������
    private Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWWEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWWEN_ITEMS * (i / NUMBER_OF_COLUMN)), 0);
    }
}

