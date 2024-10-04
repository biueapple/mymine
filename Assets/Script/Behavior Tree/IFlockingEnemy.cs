using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlockingEnemy
{
    public Enemy[] Colleague { get; }
    //public Enemy Boss { get; }

    //가까워 지면 안되는 거리
    public float Avoidance { get; }
    //avoid의 파워
    public float AvoidPower { get; }
}
