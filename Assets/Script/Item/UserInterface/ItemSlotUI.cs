using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour , IPointerDownHandler, IPointerUpHandler , IPointerEnterHandler, IPointerExitHandler , IPointerClickHandler , ISlotUI
{
    protected ItemSlot itemSlot;
    public ItemSlot ItemSlot { get { return itemSlot; } }

    protected Image icon;
    protected Text amount;
    protected Image consumeFill;

    protected float lastClickTime = 0f;
    public float LastClickTime { get => lastClickTime; }
    protected readonly float doubleClickDelay = 0.15f; // 더블 클릭 간격을 조절할 수 있는 지연 시간
    public float DoubleClickDelay { get => doubleClickDelay; }

    public enum InputState
    {
        ADD,    //추가
        CANT,   //불가능
        CAN,    //가능

    }

    public bool Give(ItemSlotUI itemSlotUI, int amount)
    {
        return itemSlot.Give(itemSlotUI.itemSlot, amount);
    }

    public bool Swap(ItemSlotUI itemSlotUI)
    {
        return itemSlot.Reference(itemSlotUI.itemSlot);
    }

    public void Init(ItemSlot itemSlot)
    {
        this.itemSlot = itemSlot;
        icon = transform.GetChild(0).GetComponent<Image>();
        amount = transform.GetChild(1).GetComponent<Text>();
        consumeFill = transform.GetChild(2).GetComponent<Image>();
        this.itemSlot.AfterUpdate += View;
        View(this.itemSlot);
    }

    protected void View(ItemSlot _itemSlot)
    {
        if (_itemSlot.Item == null)
        {
            icon.gameObject.SetActive(false);
            amount.text = "";
            consumeFill.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
            icon.sprite = _itemSlot.Item.Icon;
            amount.text = _itemSlot.Amount > 1 ? _itemSlot.Amount.ToString() : "";
            if(_itemSlot.Item is IConsume consume)
            {
                if(consume.MaxDurability > 1)
                {
                    consumeFill.gameObject.SetActive(true);
                    consumeFill.fillAmount = (float)consume.Durability / consume.MaxDurability;
                }
            }
        }
    }

    //콜백은 항상 update나 코루틴보다 빨리 호출된다
    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.ItemCatchState.EventUp(this, eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.ItemCatchState.EventDown(this, eventData);
            SlotManager.Instance.ItemCatchState.Explanation.OffExplanSlotUI();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SlotManager.Instance.SlotMouseEnter(this, eventData);
        if (SlotManager.Instance.NowState == null)
        {
            if(itemSlot.Item != null && itemSlot.Amount > 0)    
                SlotManager.Instance.ItemCatchState.Explanation.ViewItemSlotUI(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SlotManager.Instance.SlotMouseExit(this, eventData);
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.ItemCatchState.Explanation.OffExplanSlotUI();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DoubleClick(eventData);
    }

    public void DoubleClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Time.time - lastClickTime < doubleClickDelay)
            {
                if (SlotManager.Instance.NowState == SlotManager.Instance.ItemCatchState)
                    SlotManager.Instance.ItemCatchState.Collecting();
            }
            lastClickTime = Time.time;
        }
    }
}
