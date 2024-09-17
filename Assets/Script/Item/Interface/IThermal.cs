using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//열에 영향을 받는 아이템
public interface IThermal
{
    //영향을 받는 시간
    public float Thermal { get; }
    //영향을 충분히 받음
    public Item Done();
}
