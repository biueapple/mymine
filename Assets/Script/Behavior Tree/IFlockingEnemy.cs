using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlockingEnemy
{
    public Enemy[] Colleague { get; }
    //public Enemy Boss { get; }

    //����� ���� �ȵǴ� �Ÿ�
    public float Avoidance { get; }
    //avoid�� �Ŀ�
    public float AvoidPower { get; }
}
