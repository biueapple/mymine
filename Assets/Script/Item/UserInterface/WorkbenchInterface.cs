using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkbenchInterface : MonoBehaviour
{
    [SerializeField]
    private StaticInterface staticInterface;
    //â�� ������ ��� �������� ���ο��� ���ư��� ��İ� �����ִ� ��� ���� �ϳ��� ����

    public void Interlock(Storage storage)
    {
        staticInterface.Interlock(storage);
    }
}
