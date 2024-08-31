using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemCatchState : IMouseState
{
    //아이템을 잡고있는 상태이기 때문에 아이템 슬롯을 하나 보유해야 함
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
        Debug.Log("corsor 오브젝트 풀링을 사용하지 않는 생성");
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
        //인자로 오는 slot과 corsorSlot은 같은것
        //마우스에 아이템이 남아있다면 보이도록 하고 아니면 안보이도록
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
            //이론상 StorageSlotUI 가 아닌 슬롯은 여기 오지 않음
            if (slotUI.ItemSlot.Item != null)
            {
                previous = slotUI;
                inputButton = eventData.button;
                //아이템 들기
                if (inputButton == PointerEventData.InputButton.Left)
                {
                    //전부 들기 (InsertItem은 실패할 수도 있는 메소드지만 corsor슬롯은 실패가 불가능함)
                    slotUI.Give(corsor, slotUI.ItemSlot.Amount);
                }
                else if (inputButton == PointerEventData.InputButton.Right)
                {
                    //절반만 들기 (InsertItem은 실패할 수도 있는 메소드지만 corsor슬롯은 실패가 불가능함)
                    slotUI.Give(corsor, Mathf.CeilToInt(slotUI.ItemSlot.Amount * 0.5f));
                }
            }
        }
        
    }

    //up은 드래그가 끝나는지만 아는 느낌으로
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
        //아이템 놓기 
        //스왑도 가능
        //드래그 준비
        //무언가 클릭함 (사실 MonoBehaviour를 상속받지 못한 ui는 구분하지 못함)
        if (UIManager.Instance.TryGetGraphicRay(out MonoBehaviour behaviour))
        {
            //제대로 상호작용 가능한 슬롯을 클릭
            if (behaviour.TryGetComponent(out ItemSlotUI slot))
            {
                down = true;
                //전부 놓기 (스왑도 가능)
                if (!corsor.Swap(slot))
                {
                    if (corsor.Give(previous, corsor.ItemSlot.Amount))
                    {
                        Debug.Log("되돌리기 성공");
                    }
                    else
                    {
                        //아이템 획득으로 넘어감
                        Debug.Log("되돌리기 실패");
                    }
                    itemDivision.Init(null, null);
                }
                else
                {
                    itemDivision.Init(slot, corsor);
                }
            }
            //무언가 클릭은 했지만 상호작용 가능한 슬롯은 아님 (처음 슬롯으로 되돌리기)
            else
            {
                Another();
            }
        }
        //빈곳을 클릭함 (아이템 버리기)
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
                //하나만 놓기 (스왑도 가능)
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
            //무언가 클릭은 했지만 상호작용 가능한 슬롯은 아님 (처음 슬롯으로 되돌리기)
            else
            {
                Another();
            }
        }
        //빈곳을 클릭함 (아이템 버리기)
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
                //아이템을 나누고
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
        Debug.Log("손에 든 아이템 버리기");

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
                Debug.Log("되돌리기 성공");
            }
            else
            {
                Debug.Log("되돌리기 실패 던지기");
            }
            SlotManager.Instance.NowState = null;
        }
    }

    public void Collecting()
    {
        //아이템을 잡은 상태
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
    //지나쳐간 슬롯들에게 아이템을 나누기
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
    //지나쳐간 슬롯들에게 나눠준 아이템을 다시 없애기
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
            //지나쳐간 슬롯들 초기화
            _pellet.Clear();
            _pellet.Add(criteria);
            //기준이 되는 아이템과 갯수
            _criteria = new ItemSlot(true, true);
            _criteria.Update(criteria.ItemSlot.Item, criteria.ItemSlot.Amount);
            //손
            _corsor = corsor;
        }
    }
    public void DistributionCalculate(ItemSlotUI storageSlotUI)
    {
        //나눠줄 아이템보다 나눠줄 칸이 많으면 안됌
        if (_criteria == null || _criteria.Amount <= _pellet.Count)
            return;
        //새 슬롯이 들어오면
        //나눠준 아이템을 회수하고
        Taking();
        //지나쳐간 슬롯들에 추가
        _pellet.Add(storageSlotUI);
        //나눠주기
        Distribution();
    }
}

public class ItemOneByOne
{
    protected ItemSlot _criteria;
    protected ItemSlotUI _corsor;
    protected List<ItemSlotUI> _pellet = new();

    //지나쳐간 슬롯들에게 나눠준 아이템을 다시 없애기
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
            //지나쳐간 슬롯들 초기화
            _pellet.Clear();
            _pellet.Add(criteria);
            //기준이 되는 아이템과 갯수
            _criteria = new ItemSlot(true, true);
            _criteria.Update(corsor.ItemSlot.Item, corsor.ItemSlot.Amount);
            //손
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
        //나눠줄 아이템보다 나눠줄 칸이 많으면 안됌
        if (_criteria == null || _criteria.Amount <= _pellet.Count)
            return;
        //새 슬롯이 들어오면
        //나눠준 아이템을 회수하고
        Taking();
        //지나쳐간 슬롯들에 추가
        _pellet.Add(storageSlotUI);
        //나눠주기
        OneByOne();
    }
}