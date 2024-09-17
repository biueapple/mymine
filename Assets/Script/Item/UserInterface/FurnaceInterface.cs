using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceInterface : MonoBehaviour
{
    [SerializeField]
    private StaticInterface staticInterface;
    [SerializeField]
    private Image fire;
    public float Fire { set { fire.fillAmount = value; } }
    [SerializeField]
    private Image completion;
    public float Completion { set { completion.fillAmount = value ; } }
    //창을 닫으면 모든 아이템은 주인에게 돌아가는 방식과 남아있는 방식 둘중 하나를 선택

    public void Interlock(Storage storage)
    {
        staticInterface.Interlock(storage);
    }
}
