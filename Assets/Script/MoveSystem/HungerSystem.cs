using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//실제로 이동한 거리와 플레이어가 의도한 거리를 비교해서 배고픔 수치를 정함
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

    //movecallback (의도한 거리)
    public void Move(Vector3 distance)
    {
        intention = distance;
    }

    //movephysicscallback (실제 움직인 거리)
    public void MovePhysics(Vector3 distance)
    {
        physics = distance;
    }


    //플레이어는 자신이 의도한 만큼의 이동거리 이상의 배고픔을 소모하지 않음
    public void Calculation()
    {
        Vector3 distance = Vector3.Min(intention, physics);
        hunger -= distance.magnitude * 0.1f;
        //매 프레임마다 값을 정해주는게 아님 의도한 거리나 실제 움직여진 거리가 vector3.zero인 경우 콜백을 호출해 주지 않음
        //그럴경우 비교가 전의 입력된 값으로 비교가 될 수 있기에 매 비교마다 값을 초기화 해줘야 함
        intention = Vector3.zero;
        physics = Vector3.zero;
        hungerCallback?.Invoke(hunger);
    }
}
