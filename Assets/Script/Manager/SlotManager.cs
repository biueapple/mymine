using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour
{
    private static SlotManager instance;
    public static SlotManager Instance
    {
        get 
        {
            if (instance == null)
                instance = new GameObject("UISlotManager").AddComponent<SlotManager>();
            return instance; 
        }
    }

    //state���� new�� ����鼭 �־� �� ���� ������ ���ʿ� state�� ���ڰ� �����ϱ� �̸� ������� �ʱ�ȭ�� �����ִ� �������� ����� ��
    private Coroutine _inputStandby = null;
    private IMouseState nowState;
    public IMouseState NowState
    {
        get { return nowState; }
        set
        {
            nowState = value;
            if (nowState != null)
            {
                _inputStandby = StartCoroutine(InputStandbyCoroutine());
            }
            else if (_inputStandby != null)
            {
                StopCoroutine(_inputStandby);
                _inputStandby = null;
            }
        }
    }

    //
    private ItemCatchState itemCatchState;
    public ItemCatchState ItemCatchState { get { return itemCatchState; } }

    private SkillCatchState skillCatchState;
    public SkillCatchState SkillCatchState { get { return skillCatchState; } }

    private MotionCatchState motionCatchState;
    public MotionCatchState MotionCatchState { get { return motionCatchState; } }

    //

    private void Awake()
    {
        Explanation explanation = new Explanation();
        itemCatchState = new ItemCatchState(GameManager.Instance.Players[0].InventorySystem, explanation);
        skillCatchState = new SkillCatchState(explanation);
        motionCatchState = new MotionCatchState(explanation);
        //ù ���´� �Է��� ��ٸ��� ����
        nowState = null;
    }
    //�������� ��°��� down���� ���°��� up����


    private IEnumerator InputStandbyCoroutine()
    {
        while (true)
        {
            yield return null;
            if (Input.GetMouseButtonUp(0))
            {
                nowState.UpdateLeftUp();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                nowState.UpdateLeftDown();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                nowState.UpdateRightUp();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                nowState.UpdateRightDown();
            }
        }
    }

    public void SlotMouseEnter(ISlotUI slot, PointerEventData eventData)
    {
        nowState?.Enter(slot, eventData);
    }

    public void SlotMouseExit(ISlotUI slot, PointerEventData eventData)
    {
        nowState?.Exit(slot, eventData);
    }

}

public interface IMouseState
{
    //event�� ���� ȣ�� (nowState == null �϶� slot�� ȣ����)
    public void EventUp(ISlotUI slot, PointerEventData eventData);
    //event�� ���� ȣ�� (nowState == null �϶� slot�� ȣ����)
    public void EventDown(ISlotUI slot, PointerEventData eventData);
    //update�� ���� ȣ�� (nowState != null �϶� manager�� ȣ����)
    public void UpdateLeftUp();
    //update�� ���� ȣ�� (nowState != null �϶� manager�� ȣ����)
    public void UpdateLeftDown();
    //
    public void UpdateRightUp();

    public void UpdateRightDown();


    public void Enter(ISlotUI slot, PointerEventData eventData);
    public void Exit(ISlotUI slot, PointerEventData eventData);
    //
    public void Another();
}

public interface ISlotUI
{

}