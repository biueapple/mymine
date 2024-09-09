using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �̵��� �Ÿ��� �÷��̾ �ǵ��� �Ÿ��� ���ؼ� ����� ��ġ�� ����
[System.Serializable]
public class HungerSystem : IMoveCallback, IMovePhysicsCallback
{
    [SerializeField]
    private float hunger;
    public float Hunger { get { return hunger; } }
    
    [SerializeField]
    private Vector3 intention;
    [SerializeField]
    private Vector3 physics;

    private Action<float> hungerCallback;
    public Action<float> HungerCallback { get { return hungerCallback; } set { hungerCallback = value; } }

    public HungerSystem(MoveSystem moveSystem)
    {
        hunger = 20;
        moveSystem.MoveCallbacks.Add(this);
        moveSystem.MovePhysicsCallbacks.Add(this);
    }

    //movecallback (�ǵ��� �Ÿ�)
    public void Move(Vector3 distance)
    {
        intention = distance;
    }

    //movephysicscallback (���� ������ �Ÿ�)
    public void MovePhysics(Vector3 distance)
    {
        physics = distance;
    }


    //�÷��̾�� �ڽ��� �ǵ��� ��ŭ�� �̵��Ÿ� �̻��� ������� �Ҹ����� ����
    public void Calculation()
    {
        Vector3 distance = Vector3.Min(intention, physics);
        hunger -= distance.magnitude * 0.1f;
        //�� �����Ӹ��� ���� �����ִ°� �ƴ� �ǵ��� �Ÿ��� ���� �������� �Ÿ��� vector3.zero�� ��� �ݹ��� ȣ���� ���� ����
        //�׷���� �񱳰� ���� �Էµ� ������ �񱳰� �� �� �ֱ⿡ �� �񱳸��� ���� �ʱ�ȭ ����� ��
        intention = Vector3.zero;
        physics = Vector3.zero;
        hungerCallback?.Invoke(hunger);
    }
}
