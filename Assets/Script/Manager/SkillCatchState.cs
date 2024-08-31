using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillCatchState : IMouseState
{
    private SkillSlotUI previous;
    private SkillSlotUI corsor;

    private readonly Explanation explanation;
    public Explanation Explanation { get { return explanation; } }

    private Coroutine coroutine;

    public SkillCatchState(Explanation explanation)
    {
        this.explanation = explanation;
        corsor = Object.Instantiate(Resources.Load<SkillSlotUI>("UI/Slot/SkillSlotUI"), UIManager.Instance.Canvas.transform);
        Debug.Log("corsor ������ƮǮ���� ������� ���� ����");
        corsor.GetComponent<Image>().raycastTarget = false;
        corsor.gameObject.SetActive(false);
        corsor.Init();
    }

    public void Another()
    {
        if(previous != null)
        {
            previous.AntecedentSkill = corsor.AntecedentSkill;
            previous.View();
            corsor.AntecedentSkill = null;
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
        if (slot is SkillSlotUI slotUI)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if (slotUI.AntecedentSkill != null)
                {
                    previous = slotUI;

                    //corsor�� �ű��
                    corsor.AntecedentSkill = slotUI.AntecedentSkill;
                    corsor.View();
                    corsor.gameObject.SetActive(true);
                    coroutine = UIManager.Instance.MouseFallow(corsor.gameObject);

                    slotUI.AntecedentSkill = null;
                    slotUI.View();
                }
            }
            else if(eventData.button == PointerEventData.InputButton.Right)
            {
                if (slotUI.AntecedentSkill != null)
                {
                    slotUI.AntecedentSkill = null;
                    slotUI.View();
                }
            }
        }
        else if(slot is SkillView view)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                if(view.AntecedentSkill != null && view.AntecedentSkill.Skill.Level > 0)
                {
                    corsor.AntecedentSkill = view.AntecedentSkill;
                    corsor.View();
                    corsor.gameObject.SetActive(true);
                    coroutine = UIManager.Instance.MouseFallow(corsor.gameObject);
                }
            }
        }
    }

    public void EventUp(ISlotUI slot, PointerEventData eventData)
    {
        if (corsor.AntecedentSkill != null)
        {
            SlotManager.Instance.NowState = SlotManager.Instance.SkillCatchState;
        }
    }

    public void Exit(ISlotUI slot, PointerEventData eventData)
    {

    }

    public void UpdateLeftDown()
    {
        //���� Ŭ���� (��� MonoBehaviour�� ��ӹ��� ���� ui�� �������� ����)
        if (UIManager.Instance.TryGetGraphicRay(out MonoBehaviour behaviour))
        {
            //����� ��ȣ�ۿ� ������ ������ Ŭ��
            if (behaviour.TryGetComponent(out SkillSlotUI slot))
            {
                slot.AntecedentSkill = corsor.AntecedentSkill;
                slot.View();
                corsor.AntecedentSkill = null;
                corsor.View();
                corsor.gameObject.SetActive(false);
                if(coroutine != null)
                    UIManager.Instance.StopCoroutine(coroutine);
            }
            //���� Ŭ���� ������ ��ȣ�ۿ� ������ ������ �ƴ� (ó�� �������� �ǵ�����)
            else
            {
                Another();
            }
        }
        else
        {
            //�ǵ�����
            Another();
        }
    }

    public void UpdateLeftUp()
    {
        SlotManager.Instance.NowState = null;
        if (corsor.AntecedentSkill != null)
        {
            corsor.AntecedentSkill = null;
            corsor.View();
            corsor.gameObject.SetActive(false);
            if (coroutine != null)
                UIManager.Instance.StopCoroutine(coroutine);
        }
    }

    public void UpdateRightDown()
    {
        SlotManager.Instance.NowState = null;
        if (corsor.AntecedentSkill != null)
        {
            corsor.AntecedentSkill = null;
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
