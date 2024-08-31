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

    //state들을 new로 만들면서 넣어 줄 수도 있지만 애초에 state의 숫자가 적으니까 미리 만든다음 초기화를 시켜주는 형식으로 만들어 봄
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
        //첫 상태는 입력을 기다리는 상태
        nowState = null;
    }
    //아이템을 드는것은 down에서 놓는것은 up에서


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
    //event로 인한 호출 (nowState == null 일때 slot이 호출함)
    public void EventUp(ISlotUI slot, PointerEventData eventData);
    //event로 인한 호출 (nowState == null 일때 slot이 호출함)
    public void EventDown(ISlotUI slot, PointerEventData eventData);
    //update로 인한 호출 (nowState != null 일때 manager가 호출함)
    public void UpdateLeftUp();
    //update로 인한 호출 (nowState != null 일때 manager가 호출함)
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