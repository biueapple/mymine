using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AttackMotionSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, ISlotUI
{
    private IAttackMotion attackMotion;
    public IAttackMotion AttackMotion { get { return attackMotion; } set { attackMotion = value; View(); } }

    private Player player;
    private int index;

    [SerializeField]
    private Image icon;

    public void Init(Player player, int i)
    {
        this.player = player;
        index = i;
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

        if (player != null && player.StateBattle != null && player.StateBattle.AttackModule != null)
        {
            player.StateBattle.AttackModule.MotionAdd(attackMotion, index);
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
