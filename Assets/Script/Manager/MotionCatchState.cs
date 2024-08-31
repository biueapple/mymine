using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MotionCatchState : IMouseState
{
    private AttackMotionSlot previous;
    private AttackMotionSlot corsor;

    private readonly Explanation explanation;
    public Explanation Explanation { get { return explanation; } }

    private Coroutine coroutine;

    public MotionCatchState(Explanation explanation)
    {
        this.explanation = explanation;
        corsor = Object.Instantiate(Resources.Load<AttackMotionSlot>("UI/Slot/AttackSlot"), UIManager.Instance.Canvas.transform);
        corsor.GetComponent<Image>().raycastTarget = false;
        corsor.gameObject.SetActive(false);
        corsor.Init(null, 0);
    }

    public void Another()
    {
        if (previous != null)
        {
            previous.AttackMotion = corsor.AttackMotion;
            previous.View();
            corsor.AttackMotion = null;
            corsor.View();
            corsor.gameObject.SetActive(false);
            if (coroutine != null)
                UIManager.Instance.StopCoroutine(coroutine);
        }
    }

    public void Enter(ISlotUI slot, PointerEventData eventData)
    {

    }

    public void EventDown(ISlotUI slot, PointerEventData eventData)
    {
        if (slot is AttackMotionSlot slotUI)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (slotUI.AttackMotion != null)
                {
                    previous = slotUI;

                    //corsor로 옮기기
                    corsor.AttackMotion = slotUI.AttackMotion;
                    corsor.View();
                    corsor.gameObject.SetActive(true);
                    coroutine = UIManager.Instance.MouseFallow(corsor.gameObject);

                    slotUI.AttackMotion = null;
                    slotUI.View();
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (slotUI.AttackMotion != null)
                {
                    slotUI.AttackMotion = null;
                    slotUI.View();
                }
            }
        }
        else if (slot is AttackMotionView view)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (view.AttackMotion != null)
                {
                    corsor.AttackMotion = view.AttackMotion;
                    corsor.View();
                    corsor.gameObject.SetActive(true);
                    coroutine = UIManager.Instance.MouseFallow(corsor.gameObject);
                }
            }
        }
    }

    public void EventUp(ISlotUI slot, PointerEventData eventData)
    {
        if (corsor.AttackMotion != null)
        {
            SlotManager.Instance.NowState = SlotManager.Instance.MotionCatchState;
        }
    }

    public void Exit(ISlotUI slot, PointerEventData eventData)
    {

    }

    public void UpdateLeftDown()
    {
        //무언가 클릭함 (사실 MonoBehaviour를 상속받지 못한 ui는 구분하지 못함)
        if (UIManager.Instance.TryGetGraphicRay(out MonoBehaviour behaviour))
        {
            //제대로 상호작용 가능한 슬롯을 클릭
            if (behaviour.TryGetComponent(out AttackMotionSlot slot))
            {
                slot.AttackMotion = corsor.AttackMotion;
                slot.View();
                corsor.AttackMotion = null;
                corsor.View();
                corsor.gameObject.SetActive(false);
                if (coroutine != null)
                    UIManager.Instance.StopCoroutine(coroutine);
            }
            //무언가 클릭은 했지만 상호작용 가능한 슬롯은 아님 (처음 슬롯으로 되돌리기)
            else
            {
                Another();
            }
        }
        else
        {
            //되돌리기
            Another();
        }
    }

    public void UpdateLeftUp()
    {
        SlotManager.Instance.NowState = null;
        if (corsor.AttackMotion != null)
        {
            corsor.AttackMotion = null;
            corsor.View();
            corsor.gameObject.SetActive(false);
            if (coroutine != null)
                UIManager.Instance.StopCoroutine(coroutine);
        }
    }

    public void UpdateRightDown()
    {
        SlotManager.Instance.NowState = null;
        if (corsor.AttackMotion != null)
        {
            corsor.AttackMotion = null;
            corsor.View();
            corsor.gameObject.SetActive(false);
            if (coroutine != null)
                UIManager.Instance.StopCoroutine(coroutine);
        }
    }

    public void UpdateRightUp()
    {

    }
}
