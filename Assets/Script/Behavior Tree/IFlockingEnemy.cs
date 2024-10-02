using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFlockingEnemy
{
    public Enemy[] Colleague { get; }
    public Enemy Boss { get; }
}
