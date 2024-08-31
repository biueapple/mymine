using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemCatchState : IMouseState
{
    //�������� ����ִ� �����̱� ������ ������ ������ �ϳ� �����ؾ� ��
    readonly ItemSlotUI corsor;
    ItemSlotUI previous;

    bool down = false;
    PointerEventData.InputButton inputButton;

    readonly ItemDivision itemDivision;
    readonly ItemOneByOne itemOneByOne;

    private readonly Explanation explanation;
    public Explanation Explanation { get { return explanation; } }

    private readonly InventorySystem inventorySystem;

    private Coroutine coroutine;

    public ItemCatchState(InventorySystem inventorySystem, Explanation explanation)
    {
        corsor = Object.Instantiate(Resources.Load<ItemSlotUI>("UI/Slot/SlotUI"), UIManager.Instance.Canvas.transform);
        Debug.Log("corsor ������Ʈ Ǯ���� ������� �ʴ� ����");
        corsor.GetComponent<Image>().raycastTarget = false;
        corsor.gameObject.SetActive(false);
        ItemSlot itemSlot = new(true, true);
        corsor.Init(itemSlot);
        corsor.ItemSlot.AfterUpdate += CorsorSlotCheck;

        itemDivision = new ItemDivision();
        itemOneByOne = new ItemOneByOne();
        this.explanation = explanation;
        this.inventorySystem = inventorySystem;
    }

    private void CorsorSlotCheck(ItemSlot slot)
    {
        //���ڷ� ���� slot�� corsorSlot�� ������
        //���콺�� �������� �����ִٸ� ���̵��� �ϰ� �ƴϸ� �Ⱥ��̵���
        if (corsor.ItemSlot.Item != null && coroutine == null)
        {
            corsor.gameObject.SetActive(true);
            coroutine = UIManager.Instance.MouseFallow(corsor.gameObject);
        }
        else if (corsor.ItemSlot.Item == null && coroutine != null)
        {
            corsor.gameObject.SetActive(false);
            UIManager.Instance.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void EventDown(ISlotUI slot, PointerEventData eventData)
    {
        if(slot is  ItemSlotUI slotUI)
        {
            //�̷л� StorageSlotUI �� �ƴ� ������ ���� ���� ����
            if (slotUI.ItemSlot.Item != null)
            {
                previous = slotUI;
                inputButton = eventData.button;
                //������ ���
                if (inputButton == PointerEventData.InputButton.Left)
                {
                    //���� ��� (InsertItem�� ������ ���� �ִ� �޼ҵ����� corsor������ ���а� �Ұ�����)
                    slotUI.Give(corsor, slotUI.ItemSlot.Amount);
                }
                else if (inputButton == PointerEventData.InputButton.Right)
                {
                    //���ݸ� ��� (InsertItem�� ������ ���� �ִ� �޼ҵ����� corsor������ ���а� �Ұ�����)
                    slotUI.Give(corsor, Mathf.CeilToInt(slotUI.ItemSlot.Amount * 0.5f));
                }
            }
        }
        
    }

    //up�� �巡�װ� ���������� �ƴ� ��������
    public void EventUp(ISlotUI slot, PointerEventData eventData)
    {
        if (corsor.ItemSlot.Item != null)
        {
            SlotManager.Instance.NowState = SlotManager.Instance.ItemCatchState;
        }
    }

    public void UpdateLeftDown()
    {
        inputButton = PointerEventData.InputButton.Left;
        //������ ���� 
        //���ҵ� ����
        //�巡�� �غ�
        //���� Ŭ���� (��� MonoBehaviour�� ��ӹ��� ���� ui�� �������� ����)
        if (UIManager.Instance.TryGetGraphicRay(out MonoBehaviour behaviour))
        {
            //����� ��ȣ�ۿ� ������ ������ Ŭ��
            if (behaviour.TryGetComponent(out ItemSlotUI slot))
            {
                down = true;
                //���� ���� (���ҵ� ����)
                if (!corsor.Swap(slot))
                {
                    if (corsor.Give(previous, corsor.ItemSlot.Amount))
                    {
                        Debug.Log("�ǵ����� ����");
                    }
                    else
                    {
                        //������ ȹ������ �Ѿ
                        Debug.Log("�ǵ����� ����");
                    }
                    itemDivision.Init(null, null);
                }
                else
                {
                    itemDivision.Init(slot, corsor);
                }
            }
            //���� Ŭ���� ������ ��ȣ�ۿ� ������ ������ �ƴ� (ó�� �������� �ǵ�����)
            else
            {
                Another();
            }
        }
        //����� Ŭ���� (������ ������)
        else
        {
            EmptyDown();
        }
    }

    public void UpdateRightDown()
    {
        inputButton = PointerEventData.InputButton.Right;
        if (UIManager.Instance.TryGetGraphicRay(out MonoBehaviour behaviour))
        {
            if (behaviour.TryGetComponent(out ItemSlotUI slot))
            {
                down = true;
                //�ϳ��� ���� (���ҵ� ����)
                if (slot.ItemSlot.Amount != 0 && slot.ItemSlot.Item != null && slot.ItemSlot.Item.ItemID != corsor.ItemSlot.Item.ItemID)
                {
                    itemOneByOne.Init(null, null);
                    corsor.Swap(slot);
                }
                else
                {
                    itemOneByOne.Init(corsor, corsor);
                    corsor.Give(slot, 1);
                }
            }
            //���� Ŭ���� ������ ��ȣ�ۿ� ������ ������ �ƴ� (ó�� �������� �ǵ�����)
            else
            {
                Another();
            }
        }
        //����� Ŭ���� (������ ������)
        else
        {
            EmptyDown();
        }
    }

    public void UpdateLeftUp()
    {
        if (corsor.ItemSlot.Amount == 0)
        {
            SlotManager.Instance.NowState = null;
        }
        down = false;
    }

    public void UpdateRightUp()
    {
        if (corsor.ItemSlot.Amount == 0)
        {
            SlotManager.Instance.NowState = null;
        }
        down = false;
    }

    public void Enter(ISlotUI slot, PointerEventData eventData)
    {
        if (slot is ItemSlotUI slotUI)
        {
            if (down)
            {
                //�������� ������
                if (inputButton == PointerEventData.InputButton.Left)
                {
                    itemDivision.DistributionCalculate(slotUI);
                }
                else if (inputButton == PointerEventData.InputButton.Right)
                {
                    itemOneByOne.OneByOneCalculate(slotUI);
                }
            }
        }
            
    }

    public void Exit(ISlotUI slot, PointerEventData eventData)
    {
        
    }

    public void EmptyDown()
    {
        Debug.Log("�տ� �� ������ ������");

        inventorySystem.Throw(corsor.ItemSlot.Item.ItemID, corsor.ItemSlot.Amount);
        corsor.ItemSlot.Update(null, 0);
        SlotManager.Instance.NowState = null;
    }

    public void Another()
    {
        explanation.OffExplanSlotUI();
        Reversal();
    }

    public void Reversal()
    {
        if(previous != null && corsor.ItemSlot.Item != null)
        {
            if(corsor.Give(previous, corsor.ItemSlot.Amount))
            {
                Debug.Log("�ǵ����� ����");
            }
            else
            {
                Debug.Log("�ǵ����� ���� ������");
            }
            SlotManager.Instance.NowState = null;
        }
    }

    public void Collecting()
    {
        //�������� ���� ����
        if (SlotManager.Instance.NowState == SlotManager.Instance.ItemCatchState)
        {
            inventorySystem.Collecting(corsor.ItemSlot);
        }
    }
}

public class ItemDivision
{
    protected ItemSlot _criteria;
    protected ItemSlotUI _corsor;
    protected List<ItemSlotUI> _pellet = new();
    //�����İ� ���Ե鿡�� �������� ������
    protected void Distribution()
    {
        int amount = _criteria.Amount / _pellet.Count;
        int rest = _criteria.Amount % _pellet.Count;
        for (int i = 0; i < _pellet.Count; i++)
        {
            if (_pellet[i].ItemSlot.Item == null)
            {
                _pellet[i].ItemSlot.Update(_criteria.Item, amount);
            }
            else
            {
                _pellet[i].ItemSlot.Update(_criteria.Item, _pellet[i].ItemSlot.Amount + amount);
            }
        }
        _corsor.ItemSlot.Update(_criteria.Item, rest);
    }
    //�����İ� ���Ե鿡�� ������ �������� �ٽ� ���ֱ�
    protected void Taking()
    {
        for (int i = 0; i < _pellet.Count; i++)
        {
            _pellet[i].ItemSlot.Update(_pellet[i].ItemSlot.Item, _pellet[i].ItemSlot.Amount -(_criteria.Amount / _pellet.Count));
        }
        _corsor.ItemSlot.Update(_criteria.Item, _criteria.Amount);
    }

    public void Init(ItemSlotUI criteria, ItemSlotUI corsor)
    {
        if (criteria == null)
        {
            _pellet.Clear();
            _criteria = null;
            _corsor = null;
        }
        else
        {
            //�����İ� ���Ե� �ʱ�ȭ
            _pellet.Clear();
            _pellet.Add(criteria);
            //������ �Ǵ� �����۰� ����
            _criteria = new ItemSlot(true, true);
            _criteria.Update(criteria.ItemSlot.Item, criteria.ItemSlot.Amount);
            //��
            _corsor = corsor;
        }
    }
    public void DistributionCalculate(ItemSlotUI storageSlotUI)
    {
        //������ �����ۺ��� ������ ĭ�� ������ �ȉ�
        if (_criteria == null || _criteria.Amount <= _pellet.Count)
            return;
        //�� ������ ������
        //������ �������� ȸ���ϰ�
        Taking();
        //�����İ� ���Ե鿡 �߰�
        _pellet.Add(storageSlotUI);
        //�����ֱ�
        Distribution();
    }
}

public class ItemOneByOne
{
    protected ItemSlot _criteria;
    protected ItemSlotUI _corsor;
    protected List<ItemSlotUI> _pellet = new();

    //�����İ� ���Ե鿡�� ������ �������� �ٽ� ���ֱ�
    protected void Taking()
    {
        for (int i = 0; i < _pellet.Count; i++)
        {
            _pellet[i].ItemSlot.Update(_pellet[i].ItemSlot.Item, _pellet[i].ItemSlot.Amount - 1);
        }
        _corsor.ItemSlot.Update(_criteria.Item, _criteria.Amount);
    }

    public void Init(ItemSlotUI criteria, ItemSlotUI corsor)
    {
        if (criteria == null)
        {
            _pellet.Clear();
            _criteria = null;
            _corsor = null;
        }
        else
        {
            //�����İ� ���Ե� �ʱ�ȭ
            _pellet.Clear();
            _pellet.Add(criteria);
            //������ �Ǵ� �����۰� ����
            _criteria = new ItemSlot(true, true);
            _criteria.Update(corsor.ItemSlot.Item, corsor.ItemSlot.Amount);
            //��
            _corsor = corsor;
        }
    }
    protected void OneByOne()
    {
        for (int i = 0; i < _pellet.Count; i++)
        {
            if (_pellet[i].ItemSlot.Item == null)
            {
                _pellet[i].ItemSlot.Update(_criteria.Item, 1);
            }
            else
            {
                _pellet[i].ItemSlot.Update(_pellet[i].ItemSlot.Item, _pellet[i].ItemSlot.Amount + 1);
            }
        }
        _corsor.ItemSlot.Update(_criteria.Item, _criteria.Amount - _pellet.Count);
    }
    public void OneByOneCalculate(ItemSlotUI storageSlotUI)
    {
        //������ �����ۺ��� ������ ĭ�� ������ �ȉ�
        if (_criteria == null || _criteria.Amount <= _pellet.Count)
            return;
        //�� ������ ������
        //������ �������� ȸ���ϰ�
        Taking();
        //�����İ� ���Ե鿡 �߰�
        _pellet.Add(storageSlotUI);
        //�����ֱ�
        OneByOne();
    }
}