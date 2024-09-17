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
    //â�� ������ ��� �������� ���ο��� ���ư��� ��İ� �����ִ� ��� ���� �ϳ��� ����

    public void Interlock(Storage storage)
    {
        staticInterface.Interlock(storage);
    }
}
