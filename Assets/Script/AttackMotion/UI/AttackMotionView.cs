using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttackMotionView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISlotUI
{
    private IAttackMotion attackMotion;
    public IAttackMotion AttackMotion { get { return attackMotion; } }

    [SerializeField]
    private Image icon;

    public void Init(IAttackMotion attackMotion)
    {
        this.attackMotion = attackMotion;
        View();
    }

    public void View()
    {
        if (attackMotion == null)
        {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.sprite = attackMotion.Icon;
            icon.gameObject.SetActive(true);
        }
    }

    //콜백은 항상 update나 코루틴보다 빨리 호출된다
    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.MotionCatchState.EventUp(this, eventData);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.MotionCatchState.EventDown(this, eventData);
            SlotManager.Instance.MotionCatchState.Explanation.OffExplanSlotUI();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            if (attackMotion != null)
                SlotManager.Instance.MotionCatchState.Explanation.ViewMotionSlotUI(this);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (SlotManager.Instance.NowState == null)
        {
            SlotManager.Instance.MotionCatchState.Explanation.OffExplanSlotUI();
        }
    }
}
